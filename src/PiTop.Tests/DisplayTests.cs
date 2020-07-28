using System.Drawing;
using System.Drawing.Imaging;

using FluentAssertions;

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
            
            var screen = display.Capture();
            var pixel = screen.GetPixel(25, 25);

            pixel
                .Should()
                .Be(Color.FromArgb(255, 0, 0, 0));


            display.Clear(Color.Aqua);
            screen = display.Capture();
            pixel = screen.GetPixel(25, 25);
            pixel
                .Should()
                .Be(Color.FromArgb(255, Color.Aqua.R, Color.Aqua.G, Color.Aqua.B));
        }
    }
}