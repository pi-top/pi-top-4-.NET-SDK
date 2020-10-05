using System;

using FluentAssertions;

using PiTop;
using PiTop.Tests;

using PiTop.MakerArchitecture.Foundation;

using Xunit;

namespace PiTop.MakerArchitecture.Expansion.Tests
{
    public class MmkPlateTests : IDisposable
    {
        private readonly PiTop4Board _module;
        public MmkPlateTests()
        {
            PiTop4Board.Configure(new DummyGpioController());
            _module = PiTop4Board.Instance;
        }

        [Fact]
        public void can_obtain_plate_from_module()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            plate.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_servo()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var led = plate.GetOrCreateDevice<ServoMotor>(ServoMotorPort.S1);

            led.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_motor()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var led = plate.GetOrCreateDevice<EncoderMotor>(EncoderMotorPort.M1);

            led.Should().NotBeNull();
        }

        public void Dispose()
        {
            _module.Dispose();
        }
    }
}