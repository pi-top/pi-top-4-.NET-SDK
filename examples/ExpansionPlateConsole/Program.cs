using System;
using System.Threading.Tasks;

using PiTop;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Sensors;
using Spectre.Console;
using Pocket;

namespace ExpansionPlateConsole
{
    public sealed class SensorValue : IBarChartItem
    {
        public string Label { get; set; }
        public double Value { get; set; }
        public Color? Color { get; set; }

        public SensorValue(string label, double value, Color? color = null)
        {
            Label = label;
            Value = value;
            Color = color;
        }
    }
    class Program
    {

        static async Task Main(string[] args)
        {
            using var operation = Logger.Log.OnEnterAndExit();

            LogEvents.Subscribe(i =>
            {
                i.Operation.Id = "";
                AnsiConsole.WriteLine(i.ToLogString());
            }, new[]
            {
                typeof(PiTop4Board).Assembly,
                typeof(FoundationPlate).Assembly,
                typeof(ExpansionPlate).Assembly,
                typeof(Program).Assembly,
            });
            UltrasonicSensor frontUltrasound = null; 
            UltrasonicSensor backUltrasound = null;
            var frontSensorValue = new SensorValue("Front", 0, Color.Green);
            var backSensorValue = new SensorValue("Back", 0, Color.Green);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                operation.Info("Creating pi-top device");
                var expansionPlate = PiTop4Board.Instance.GetOrCreateExpansionPlate();
                frontUltrasound = expansionPlate.GetOrCreateUltrasonicSensor(AnaloguePort.A1);

                backUltrasound = expansionPlate.GetOrCreateUltrasonicSensor(AnaloguePort.A3);              
            }

            var rnd = new Random();

     

            var chart = new BarChart()
.Width(60)
.Label("[green bold underline]Ultrasound readings[/]")
.CenterLabel()
.AddItem(frontSensorValue)
.AddItem(backSensorValue);

            await AnsiConsole.Live(chart)
                .StartAsync(async ctx =>
                {

                    while (true)
                    {
                        await Task.Delay(500);

                        frontSensorValue.Value = frontUltrasound?.Distance.Centimeters ?? rnd.Next(1, 50);
                        backSensorValue.Value = backUltrasound?.Distance.Centimeters ?? rnd.Next(1, 50);

                        chart.Data.Clear();

                        chart.AddItem(frontSensorValue)
                        .AddItem(backSensorValue);

                        ctx.UpdateTarget(chart);
                    }
                });
        }
    }
}
