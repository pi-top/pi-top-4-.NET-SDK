using FluentAssertions;

using Xunit;

namespace PiTop.Algorithms.Tests
{
    public class InterpolationTests
    {
        [Theory]
        [InlineData(2, 1, 3, 5, 7, 6)]
        [InlineData(-2, 1, 3, 5, 7, 5)]
        [InlineData(200, 1, 3, 5, 7, 7)]
        public void interpolation_tests(double point, double domainMin, double domainMax, double codomainMin, double codomainMax, double expectedResult)
        {
            point.Interpolate(domainMin, domainMax, codomainMin, codomainMax).Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(2, new double[] { 1, 3 }, new double[] { 5, 7 }, 6)]
        [InlineData(-2, new double[] { 1, 3 }, new double[] { 5, 7 }, 5)]
        [InlineData(200, new double[] { 1, 3 }, new double[] { 5, 7 }, 7)]
        [InlineData(1, new double[] { -2, -1, 1, 3 }, new double[] { -5, 0, 0, 7 }, 0)]
        [InlineData(0, new double[] { -2, -1, 1, 3 }, new double[] { -5, 0, 0, 7 }, 0)]
        [InlineData(-1.5, new double[] { -2, -1, 1, 3 }, new double[] { -5, 0, 0, 7 }, -2.5)]
        [InlineData(-3, new double[] { -2, -1, 1, 3 }, new double[] { -5, 0, 0, 7 }, -5)]
        [InlineData(2, new double[] { -2, -1, 1, 3 }, new double[] { -5, 0, 0, 7 }, 3.5)]
        [InlineData(4, new double[] { -2, -1, 1, 3 }, new double[] { -5, 0, 0, 7 }, 7)]
        public void more_complex_domain_interpolation_tests(double point, double[] domain, double[] codomain, double expectedResult)
        {
            point.Interpolate(domain, codomain).Should().Be(expectedResult);
        }

    }
}