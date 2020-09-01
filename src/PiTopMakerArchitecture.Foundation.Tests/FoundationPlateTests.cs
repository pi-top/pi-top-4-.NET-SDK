using System;
using FluentAssertions;

using PiTop;
using PiTop.Tests;

using PiTopMakerArchitecture.Foundation.Components;
using PiTopMakerArchitecture.Foundation.Sensors;
using Xunit;

namespace PiTopMakerArchitecture.Foundation.Tests
{
    public class FoundationPlateTests: IDisposable
    {
        private readonly PiTop4Board _module;

        public FoundationPlateTests()
        {
            PiTop4Board.Configure(new DummyGpioController());
            _module = PiTop4Board.Instance;
        }
        [Fact]
        public void can_obtain_plate_from_module()
        {
            using var plate = _module.GetOrCreatePlate<FoundationPlate>();

            plate.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_create_led()
        {
            using var plate = _module.GetOrCreatePlate<FoundationPlate>();

            using var led = plate.GetOrCreateDevice<Led>(DigitalPort.D0);

            led.Should().NotBeNull();
        }

        [Fact]
        public void plate_returns_previously_created_devices()
        {
            using var plate = _module.GetOrCreatePlate<FoundationPlate>();

            var led1 = plate.GetOrCreateDevice<Led>(DigitalPort.D0);
            var led2 = plate.GetOrCreateDevice<Led>(DigitalPort.D0);

            led2.Should().BeSameAs(led1);
        }

        [Fact]
        public void cannot_create_a_different_device_on_allocated_pins()
        {
            using var plate = _module.GetOrCreatePlate<FoundationPlate>();

           plate.GetOrCreateDevice<Led>(DigitalPort.D0);

            var action = new Action(() =>
            {
               plate.GetOrCreateDevice<UltrasonicSensor>(DigitalPort.D0);
            });

            action.Should().Throw<InvalidOperationException>()
                .Which
                .Message
                .Should().Match("Connection D0 is already used by PiTopMakerArchitecture.Foundation.Components.Led device");
        }

        public void Dispose()
        {
            _module.Dispose();
        }
    }
}
