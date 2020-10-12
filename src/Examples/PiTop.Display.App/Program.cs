using System;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace PiTop.Display.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using var module = PiTop4Board.Instance;

            var display = module.Display;

            Console.WriteLine("press enter key to render square");
            Console.ReadLine();
            display.Draw((d,cr) =>
            {
                var square = new RectangularPolygon(0, 0, cr.Width/2, cr.Height/2);
     
                d.Fill(Color.White, square);
            });

            Console.WriteLine("press enter key to render text");
            Console.ReadLine();

            var font = SystemFonts.Collection.Find("Roboto").CreateFont(10);
            module.Display.Draw((context,cr) => {
                context.Clear(Color.Black);
                var rect = TextMeasurer.Measure("Diego was here", new RendererOptions(font));
                var x = (cr.Width - rect.Width) / 2;
                var y = (cr.Height + (rect.Height)) / 2;
                context.DrawText("Hello\nWorld", font, Color.White, new PointF(0, 0));
            });

            Console.WriteLine("press enter key to exit");
            Console.ReadLine();
        }
    }
}
