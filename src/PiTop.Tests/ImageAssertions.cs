using FluentAssertions;
using FluentAssertions.Primitives;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace PiTop.Tests
{
    internal static class ImageAssertions
    {
        public static AndConstraint<ObjectAssertions> ShouldAllPixelBe<TPixel>(this Image<TPixel> actual, Color expectedColor) where TPixel : unmanaged, IPixel<TPixel>
        {
            for (var y = 0; y < actual.Height; y++)
            {
                var row = actual.GetPixelRowSpan(y);

                for (var x = 0; x < actual.Width; x++)
                {
                    var actualPixel = row[x];
                    var expectedPixel = expectedColor.ToPixel<TPixel>();
                    actualPixel.Should().BeEquivalentTo(expectedPixel);
                }
            }
            return new AndConstraint<ObjectAssertions>(actual.Should());
        }
    }
}