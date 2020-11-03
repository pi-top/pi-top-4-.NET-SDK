using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace StreamCapture
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var energies = new[]
            {
                "no energy",
                "darkness",
                "fairy",
                "fighting",
                "fire",
                "grass",
                "lightning",
                "metal",
                "psychic",
                "rainbow",
                "water"
            };

            var destination = new DirectoryInfo(args[1]);
            var client = new HttpClient();

            foreach (var energy in energies)
            {
                destination.CreateSubdirectory(energy);
            }

            var subfolder = destination.GetDirectories().Where(f => !f.Name.Contains("no energy")).OrderBy(f => f.Name).ToArray();
            Console.WriteLine($"[space] - {destination.Name}");
            Console.WriteLine($"[esc] - no energy");
            for (var i = 0; i < subfolder.Length; i++)
            {
                Console.WriteLine($"[{i}] - {subfolder[i].Name}");
            }

            var noEnergy = destination.GetDirectories("no energy").FirstOrDefault();
            var url = $"{args[0]}/?action=snapshot";
            while (true)
            {
                var key = Console.ReadKey(true);
                var focus = (key.Modifiers & ConsoleModifiers.Alt) != 0;  
                switch (key.Key)
                {

                    case ConsoleKey.Enter:
                        Console.WriteLine("done for today.");
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.Spacebar:
                        await SaveImage(destination, client, url, focus);
                        break;

                    case ConsoleKey.Escape:
                        await SaveImage(noEnergy, client, url, focus);
                        break;

                    default:
                        if (int.TryParse(key.KeyChar.ToString(), out var i))
                        {
                            await SaveImage(subfolder[i], client, url, focus);
                        }
                        break;

                }
            }
        }

        private static async Task SaveImage(DirectoryInfo destination, HttpClient client, string url, bool focus)
        {
            var data = await client.GetByteArrayAsync(url);
            var image = Image.Load(data);
            if (focus)
            {
                image = Focus(image);
            }
            var imageFilePath = Path.Combine(destination.FullName,
                $"{DateTimeOffset.Now.Year:0000}{DateTimeOffset.Now.Month:00}{DateTimeOffset.Now.Day:00}{DateTimeOffset.Now.Hour:00}{DateTimeOffset.Now.Minute:00}{DateTimeOffset.Now.Second:00}.png");

            
            await image.SaveAsync(imageFilePath);
        }

        public static Image<TPixel> Focus<TPixel>(Image<TPixel> source, bool asSquare = true) where TPixel : unmanaged, IPixel<TPixel>
        {
            var rect = CreateFocusRectangle(source, asSquare);
            return source.Clone(c => c.Crop(rect));
        }
        private static Rectangle CreateFocusRectangle(Image source, bool asSquare)
        {
            var width = source.Width / 2;
            var height = source.Height / 2;
            if (asSquare)
            {
                width = height = Math.Min(height, width);
            }
            var x = (source.Width - width) / 2;
            var y = (source.Height - height) / 2;
            var rect = new Rectangle(x, y, width, height);
            return rect;
        }

    }
}
