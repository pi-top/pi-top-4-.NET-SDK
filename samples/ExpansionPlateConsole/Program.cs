using System;
using System.Threading;
using System.Threading.Tasks;

using PiTop;
using PiTop.MakerArchitecture.Expansion;
using PiTop.MakerArchitecture.Foundation;
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

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                using var _ = Logger.Log.OnEnterAndConfirmOnExit("Device is on");
                using var expansionPlate = PiTop4Board.Instance.GetOrCreateExpansionPlate();
                var frontUltrasound = expansionPlate.GetOrCreateUltrasonicSensor(AnaloguePort.A1);

                var backUltrasound = expansionPlate.GetOrCreateUltrasonicSensor(AnaloguePort.A3);

                AnsiConsole.Markup($"[underline red]Front[/] {frontUltrasound.Distance}");
                AnsiConsole.Markup($"[underline red]Back[/] {backUltrasound.Distance}!");

                while (true)
                {
                    await Task.Delay(500);
                   // AnsiConsole.Markup($"[underline red]Front[/] {frontUltrasound.Distance}");
                   // AnsiConsole.Markup($"[underline red]Back[/] {backUltrasound.Distance}!");

                }
            }

            var rnd = new Random();
            var frontSensorValue = new SensorValue("Front", rnd.Next(1, 50), Color.Green);
            var backSensorValue = new SensorValue("Back", rnd.Next(1, 50), Color.Green);
            
            var chart = new BarChart()
                .Width(60)
                .Label("[green bold underline]Ultrasound readings[/]")
                .CenterLabel()
                .AddItem(frontSensorValue)
                .AddItem(backSensorValue);


            await AnsiConsole.Live(chart)
                .Start(async ctx =>
                {

                    while (true)
                    {
                        await Task.Delay(500);
                        frontSensorValue.Value = rnd.Next(1, 50);
                        backSensorValue.Value = rnd.Next(1, 50);

                        chart
                        .AddItem(frontSensorValue)
                        .AddItem(backSensorValue);
                        ctx.Refresh();

                    }

                });

            

        }
    }
}
