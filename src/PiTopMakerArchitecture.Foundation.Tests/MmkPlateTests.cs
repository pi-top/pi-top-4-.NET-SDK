using System;
using FluentAssertions;
using PiTop;
using PiTop.Tests;
using PiTopMakerArchitecture.Foundation.Components;
using Xunit;

namespace PiTopMakerArchitecture.Foundation.Tests
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
            using var plate = _module.GetOrCreatePlate<MmkPlate>();

            plate.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_servo()
        {
            using var plate = _module.GetOrCreatePlate<MmkPlate>();

            using var led = plate.GetOrCreateDevice<EncodedServo>(EncodedServoPort.S1);

            led.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_motor()
        {
            using var plate = _module.GetOrCreatePlate<MmkPlate>();

            using var led = plate.GetOrCreateDevice<Motor>(MotorPort.M1);

            led.Should().NotBeNull();
        }

        public void Dispose()
        {
            _module.Dispose();
        }
    }
}