using FluentAssertions;
using Xunit;

namespace PiTop.MakerArchitecture.Expansion.Tests
{
    public class MathHelperTests
    {
        [Theory]
        [InlineData(2, 1, 3, 5, 7, 6)]
        [InlineData(-2, 1, 3, 5, 7, 5)]
        [InlineData(200, 1, 3, 5, 7, 7)]
        public void interpolation_tests(double point, double domainMin, double domainMax, double codomainMin, double codomainMax, double expectedResult)
        {
            point.Interpolate(domainMin, domainMax, codomainMin, codomainMax).Should().Be(expectedResult);
        }
    }
}