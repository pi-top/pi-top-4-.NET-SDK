using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace PiTop.Display.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using var module = new PiTopModule();

            var display = module.Display;

            display.Draw(d =>
            {
                var square = new RectangularPolygon(display.Width / 4, display.Height / 4, display.Width/2, display.Height/2);
     
                d.Fill(Color.White, square);
            });
        }
    }
}
