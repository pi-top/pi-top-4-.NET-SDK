using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using SixLabors.ImageSharp;

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
                switch (key.Key)
                {

                    case ConsoleKey.Enter:
                        Console.WriteLine("done for today.");
                        Environment.Exit(0);
                        break;

                    case ConsoleKey.Spacebar:
                        await SaveImage(destination, client, url);
                        break;

                    case ConsoleKey.Escape:
                        await SaveImage(noEnergy, client, url);
                        break;

                    default:
                        if (int.TryParse(key.KeyChar.ToString(), out var i))
                        {
                            await SaveImage(subfolder[i], client, url);
                        }
                        break;

                }
            }
        }

        private static async Task SaveImage(DirectoryInfo destination, HttpClient client, string url)
        {
            var data = await client.GetByteArrayAsync(url);
            var image = Image.Load(data);
            var imageFilePath = Path.Combine(destination.FullName,
                $"{DateTimeOffset.Now.Year:0000}{DateTimeOffset.Now.Month:00}{DateTimeOffset.Now.Day:00}{DateTimeOffset.Now.Hour:00}{DateTimeOffset.Now.Minute:00}{DateTimeOffset.Now.Second:00}.png");

            
            await image.SaveAsync(imageFilePath);
        }


    }
}
