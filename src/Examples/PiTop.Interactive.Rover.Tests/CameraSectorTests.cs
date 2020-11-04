using System.Linq;

using FluentAssertions;
using FluentAssertions.Execution;

using PiTop.Interactive.Rover.ML;

using UnitsNet;

using Xunit;

namespace PiTop.Interactive.Rover.Tests
{
    public class CameraSectorTests
    {
        [Fact]
        public void sectors_list_creation()
        {
            var sectors = CameraSector.CreateSectors(3, 3, Angle.FromDegrees(-90), Angle.FromDegrees(90),
                Angle.FromDegrees(-90), Angle.FromDegrees(90)).ToArray();

            using var _ = new AssertionScope();

            sectors.Length.Should().Be(9);
            sectors[0].Pan.Should().Be(Angle.FromDegrees(-90));
            sectors[1].Pan.Should().Be(Angle.FromDegrees(0));
            sectors[2].Pan.Should().Be(Angle.FromDegrees(90));

            sectors[0].Tilt.Should().Be(Angle.FromDegrees(-90));
            sectors[1].Tilt.Should().Be(Angle.FromDegrees(-90));
            sectors[2].Tilt.Should().Be(Angle.FromDegrees(-90));

            sectors[3].Pan.Should().Be(Angle.FromDegrees(-90));
            sectors[4].Pan.Should().Be(Angle.FromDegrees(0));
            sectors[5].Pan.Should().Be(Angle.FromDegrees(90));

            sectors[3].Tilt.Should().Be(Angle.FromDegrees(0));
            sectors[4].Tilt.Should().Be(Angle.FromDegrees(0));
            sectors[5].Tilt.Should().Be(Angle.FromDegrees(0));

            sectors[6].Pan.Should().Be(Angle.FromDegrees(-90));
            sectors[7].Pan.Should().Be(Angle.FromDegrees(0));
            sectors[8].Pan.Should().Be(Angle.FromDegrees(90));

            sectors[6].Tilt.Should().Be(Angle.FromDegrees(90));
            sectors[7].Tilt.Should().Be(Angle.FromDegrees(90));
            sectors[8].Tilt.Should().Be(Angle.FromDegrees(90));

        }

        [Fact]
        public void sectors_list_distinct_elements()
        {
            var sectors = CameraSector.CreateSectors(3, 3, Angle.FromDegrees(-90), Angle.FromDegrees(-90),
                Angle.FromDegrees(-90), Angle.FromDegrees(90)).Distinct().ToArray();

            using var _ = new AssertionScope();

            sectors.Length.Should().Be(3);
            sectors[0].Pan.Should().Be(Angle.FromDegrees(-90));
            sectors[1].Pan.Should().Be(Angle.FromDegrees(-90));
            sectors[2].Pan.Should().Be(Angle.FromDegrees(-90));

            sectors[0].Tilt.Should().Be(Angle.FromDegrees(-90));
            sectors[1].Tilt.Should().Be(Angle.FromDegrees(0));
            sectors[2].Tilt.Should().Be(Angle.FromDegrees(90));

        }
    }
}
