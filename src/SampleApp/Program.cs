using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiTop;
using PiTopMakerArchitecture.Foundation;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;
using SixLabors.ImageSharp;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("ready");

            Console.WriteLine(@"Select one of the options
0 module test
1 potentiometer test
2 button test
3 led test
4 ultrasound test
5 semaphore test");

            var read = Console.ReadKey();

            switch (read.KeyChar)
            {
                case '0':
                    await TestBoard();
                    break;
                case '1':
                    await TestPotentiometer(AnaloguePort.A0);
                    break;
                case '2':
                    await TestButton(DigitalPort.D4, DigitalPort.D0.GetDigitalPortRange(3).ToArray());
                    break;
                case '3':
                    await TestLed01();
                    break;
                case '4':
                    await TestUltrasoundSensor();
                    break;
                case '5':
                    await TestSemaphore(DigitalPort.D3, DigitalPort.D0, DigitalPort.D1, DigitalPort.D2, 40, 20, 5);
                    break;
                default:
                    Console.WriteLine("invalid option");
                    break;
            }

            Console.WriteLine("done");
        }

        private static Task TestBoard()
        {
            using var board = PiTop4Board.Instance;

            Console.WriteLine(board.BatteryState);

            return  Task.CompletedTask;
        }

        private static Task TestPotentiometer(AnaloguePort port)
        {
            var cancellationSource = new CancellationTokenSource();
            
            var board = PiTop4Board.Instance;
            var plate = board.GetOrCreatePlate<FoundationPlate>();
           
            Task.Run(() =>
            {
                var potentiometer = plate.GetOrCreateDevice<Potentiometer>(port);

                Observable
                    .Interval(TimeSpan.FromSeconds(0.5))
                    .Select(_ => potentiometer.Position)
                    .Subscribe(Console.WriteLine);
             
                Console.WriteLine("press enter key to exit");
            }, cancellationSource.Token);

            return Task.Run(() =>
            {
                Console.ReadLine();
                board.Dispose();
                cancellationSource.Cancel(false);
            }, cancellationSource.Token);
        }

        private static Task TestButton(DigitalPort buttonPort, DigitalPort[] ledPorts)
        {
            var cancellationSource = new CancellationTokenSource();
            var board = PiTop4Board.Instance;
            var plate = board.GetOrCreatePlate<FoundationPlate>();

            Task.Run(() =>
            {

                var button = plate.GetOrCreateDevice<Button>(buttonPort);
                
                foreach (var digitalPort in ledPorts)
                {
                    plate.GetOrCreateDevice<Led>(digitalPort);
                }

                var leds = plate.DigitalDevices.OfType<Led>().ToArray();

                var buttonStream = Observable
                    .FromEventPattern<bool>(h => button.PressedChanged += h, h => button.PressedChanged -= h);
                var pos = -1;
                buttonStream
                    .Where(e => e.EventArgs)
                    .Select(_ =>
                    {
                        var next = ((pos + 1) % leds.Length);
                        var pair = new { Prev = pos, Next = ((pos + 1) % leds.Length) };
                        pos = next;
                        return pair;
                    })
                    .Subscribe(p =>
                    {
                        if (p.Prev >= 0)
                        {
                            leds[p.Prev].Off();
                        }
                        leds[p.Next].On();
                    });

                Console.WriteLine("press enter key to exit");
            }, cancellationSource.Token);

            return Task.Run(() =>
            {
                Console.ReadLine();
                board.Dispose();
                cancellationSource.Cancel(false);
            }, cancellationSource.Token);

        }

        private static Task TestSemaphore(DigitalPort ultrasonicSensorPort, DigitalPort greenLedPort, DigitalPort yellowLedPort, DigitalPort redLedPort, int greenThreshold, int yellowThreshold, int redThreshold)
        {
            var board = PiTop4Board.Instance;
            var plate = board.GetOrCreatePlate<FoundationPlate>();

            var cancellationSource = new CancellationTokenSource();
            var greenLed = plate.GetOrCreateLed(greenLedPort, Color.Green);
            var yellowLed = plate.GetOrCreateLed(yellowLedPort, Color.Yellow);
            var redLed = plate.GetOrCreateLed(redLedPort,Color.Red);

            ClearLeds();
            Task.Run(() =>
            {
                var sensor = plate.GetOrCreateDevice<UltrasonicSensor>(ultrasonicSensorPort);
                Observable
                    .Interval(TimeSpan.FromSeconds(0.5))
                    .Subscribe(_ =>
                    {
                        var distance = sensor.Distance.Value;


                        switch (distance)
                        {
                            case { } x when x > greenThreshold:
                                greenLed.On();
                                yellowLed.Off();
                                redLed.Off();
                                break;

                            case { } x when x < greenThreshold && x > yellowThreshold:
                                greenLed.Off();
                                yellowLed.On();
                                redLed.Off();
                                break;

                            case { } x when x < redThreshold:
                                greenLed.Off();
                                yellowLed.Off();
                                redLed.On();
                                break;
                        }
                    });

                Console.WriteLine("press enter key to exit");

            }, cancellationSource.Token);


            return Task.Run(() =>
            {
                Console.ReadLine();
                board.Dispose();
                ClearLeds();
                cancellationSource.Cancel(false);
            }, cancellationSource.Token);

            void ClearLeds()
            {
                greenLed.Off();
                yellowLed.Off();
                redLed.Off();
            }
        }

        private static Task TestUltrasoundSensor()
        {
            var board = PiTop4Board.Instance;
            var plate = board.GetOrCreatePlate<FoundationPlate>();

            var cancellationSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                var sensor =
                    plate.GetOrCreateDevice(DigitalPort.D3, (dp,c) => new UltrasonicSensor(dp,c));
                Observable
                    .Interval(TimeSpan.FromSeconds(0.5))
                    .Subscribe(_ => { Console.WriteLine(sensor.Distance); });

                Console.WriteLine("press enter key to exit");
            }, cancellationSource.Token);

            return Task.Run(() =>
                {
                    Console.ReadLine();
                    board.Dispose();
                    cancellationSource.Cancel(false);
                }, cancellationSource.Token);
        }

        private static async Task TestLed01()
        {
            var module = PiTop4Board.Instance;
            var plate = module.GetOrCreatePlate<FoundationPlate>();

            var ports = DigitalPort.D0.GetDigitalPortRange(3);
            var leds = ports
                .Select(p => plate.GetOrCreateDevice(p, (dp,c) => new Led(dp,c)))
                .ToArray();

            foreach (var led in leds)
            {
                led.Off();
            }

            var pos = 0;
            for (var i = 0; i < leds.Length * 10; i++)
            {
                leds[pos].Toggle();
                pos = (pos + 1) % 3;
                await Task.Delay(300);
            }

            foreach (var led in leds)
            {
                led.Off();
            }
        }
    }
}