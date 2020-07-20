using FluentAssertions;

using PiTop;
using PiTop.Tests;

using PiTopMakerArchitecture.Foundation.Components;

using Xunit;

namespace PiTopMakerArchitecture.Foundation.Tests
{
    public class FoundationPlateTests
    {
        [Fact]
        public void can_obtain_plate_from_module()
        {
            using var module = new PiTopModule(new DummyGpioController());

            using var plate = module.GetOrCreatePlate<FoundationPlate>();

            plate.Should().NotBeNull();
        }

        [Fact]
        public void plate_can_create_led()
        {
            using var module = new PiTopModule(new DummyGpioController());

            using var plate = module.GetOrCreatePlate<FoundationPlate>();

            using var led = plate.GetOrCreateDevice<Led>(DigitalPort.D0);

            led.Should().NotBeNull();
        }

        [Fact]
        public void plate_returns_previously_created_devices()
        {
            using var module = new PiTopModule(new DummyGpioController());

            using var plate = module.GetOrCreatePlate<FoundationPlate>();

            var led1 = plate.GetOrCreateDevice<Led>(DigitalPort.D0);
            var led2 = plate.GetOrCreateDevice<Led>(DigitalPort.D0);

            led2.Should().BeSameAs(led1);
        }
    }
}
