using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using PiTopMakerArchitecture.Foundation;
using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.InteractiveExtension;
using PiTopMakerArchitecture.Foundation.Sensors;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("ready");
            await TestPotentiometer(AnaloguePort.A0);
            //await TestButton(DigitalPort.D4, DigitalPort.D0.GetDigitalPortRange(3).ToArray());
            //await TestLed01();
            //await TestUltrasoundSensor();
            //await TestSemaphore(DigitalPort.D3, DigitalPort.D0, DigitalPort.D1, DigitalPort.D2, 40, 20, 5);
            Console.WriteLine("done");
        }

        private static Task TestPotentiometer(AnaloguePort port)
        {
            var cancellationSource = new CancellationTokenSource();
            var plate = new Plate();
            var json = plate.ToJObject();
            Task.Run(() =>
            {
                var potentiometer = plate.GetOrCreateAnalogueDevice<Potentiometer>(port);

                Observable
                    .Interval(TimeSpan.FromSeconds(0.5))
                    .Select(_ => potentiometer.Position)
                    .Subscribe(Console.WriteLine);

                Console.WriteLine("press enter key to exit");
            }, cancellationSource.Token);

            return Task.Run(() =>
            {
                Console.ReadLine();
                plate.Dispose();
                cancellationSource.Cancel(false);
            }, cancellationSource.Token);

        }

        private static Task TestButton(DigitalPort buttonPort, DigitalPort[] ledPorts)
        {
            var cancellationSource = new CancellationTokenSource();
            var plate = new Plate();
            Task.Run(() =>
            {

                var button = plate.GetOrCreateDigitalDevice<Button>(buttonPort);
                var leds = ledPorts.Select(ledPort => plate.GetOrCreateDigitalDevice<Led>(ledPort)).ToArray();
                foreach (var led in leds)
                {
                    led.Off();
                }

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
                plate.Dispose();
                cancellationSource.Cancel(false);
            }, cancellationSource.Token);

        }

        private Task TestSemaphore(DigitalPort ultrasonicSensorPort, DigitalPort greenLedPort, DigitalPort yellowLedPort, DigitalPort redLedPort, int greenThreshold, int yellowThreshold, int redThreshold)
        {
            var plate = new Plate();

            var cancellationSource = new CancellationTokenSource();
            var greenLed = plate.GetOrCreateDigitalDevice<Led>(greenLedPort);
            var yellowLed = plate.GetOrCreateDigitalDevice<Led>(yellowLedPort);
            var redLed = plate.GetOrCreateDigitalDevice<Led>(redLedPort);
            ClearLeds();
            Task.Run(() =>
            {
                var sensor = plate.GetOrCreateDigitalDevice<UltrasonicSensor>(ultrasonicSensorPort);
                Observable
                    .Interval(TimeSpan.FromSeconds(0.5))
                    .Subscribe(_ =>
                    {
                        var distance = sensor.Distance;


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
                plate.Dispose();
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
            var plate = new Plate();

            var cancellationSource = new CancellationTokenSource();
            Task.Run(() =>
            {
                var sensor =
                    plate.GetOrCreateDigitalDevice<UltrasonicSensor>(DigitalPort.D3, (dp) => new UltrasonicSensor(dp));
                Observable
                    .Interval(TimeSpan.FromSeconds(0.5))
                    .Subscribe(_ => { Console.WriteLine(sensor.Distance); });
                Console.WriteLine("press enter key to exit");
            }, cancellationSource.Token);

            return Task.Run(() =>
                {
                    Console.ReadLine();
                    plate.Dispose();
                    cancellationSource.Cancel(false);
                }, cancellationSource.Token);

        }

        private static async Task TestLed01()
        {
            using var plate = new Plate();
            var ports = DigitalPort.D0.GetDigitalPortRange(3);
            var leds = ports
                .Select(p => plate.GetOrCreateDigitalDevice(p, (dp) => new Led(dp)))
                .ToArray();

            foreach (var led in leds)
            {
                led.Off();
            }

            var pos = 0;
            for (var i = 0; i < (leds.Length * 10); i++)
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