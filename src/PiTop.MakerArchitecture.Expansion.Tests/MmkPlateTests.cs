using System;

using FluentAssertions;

using PiTop.Tests;

using Xunit;

namespace PiTop.MakerArchitecture.Expansion.Tests
{
    public class MmkPlateTests : IDisposable
    {
        private readonly PiTop4Board _module;
        public MmkPlateTests()
        {
            PiTop4Board.Configure(new DummyGpioController(), i2cDeviceFactory:settings => new DummyI2CDevice(settings));
            _module = PiTop4Board.Instance;
        }

        [Fact]
        public void can_obtain_plate_from_module()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            plate.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_create_servo()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var servoMotor = plate.GetOrCreateDevice<ServoMotor>(ServoMotorPort.S1);

            servoMotor.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_create_motor()
        {
            using var plate = _module.GetOrCreatePlate<ExpansionPlate>();

            using var encoderMotor = plate.GetOrCreateDevice<EncoderMotor>(EncoderMotorPort.M1);

            encoderMotor.Should().NotBeNull();
        }

        public void Dispose()
        {
            _module.Dispose();
        }
    }
}