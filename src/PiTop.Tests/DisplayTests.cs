using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

namespace PiTop.Tests
{
    public class DisplayTests
    {
        [Fact]
        public void can_clear_the_screen()
        {
            using var display = new HeadlessDisplay(50, 50);

            display.Clear(Color.Black);
            
            var screen = display.Capture().CloneAs<Rgba32>();

            screen.ShouldAllPixelBe(Color.Black);

            display.Clear(Color.Aqua);

            screen = display.Capture().CloneAs<Rgba32>();

            screen.ShouldAllPixelBe(Color.Aqua);
        }
    }
}