using FluentAssertions;
using FluentAssertions.Execution;
using PiTop.Abstractions;
using Xunit;

namespace PiTop.Tests
{
    public class SharedControllerTests
    {
        [Fact]
        public void tracks_open_pins()
        {
            using var source = new DummyGpioController();
            using var shared = source.Share();

            shared.OpenPin(1);

            using var _ = new AssertionScope();

            source.IsPinOpen(1).Should().BeTrue();
            shared.IsPinOpen(1).Should().BeTrue();
        }

        [Fact]
        public void distinct_share_controllers_tracks_their_own_open_pins()
        {
            using var source = new DummyGpioController();
            using var firstSharedController = source.Share();
            using var secondSharedController = source.Share();

            firstSharedController.OpenPin(1);
            secondSharedController.OpenPin(2);

            using var _ = new AssertionScope();

            source.IsPinOpen(1).Should().BeTrue();
            source.IsPinOpen(2).Should().BeTrue();

            firstSharedController.IsPinOpen(1).Should().BeTrue();
            firstSharedController.IsPinOpen(2).Should().BeFalse();

            secondSharedController.IsPinOpen(1).Should().BeFalse();
            secondSharedController.IsPinOpen(2).Should().BeTrue();
        }

        [Fact]
        public void disposes_only_tracked_pins()
        {
            using var source = new DummyGpioController();
            var firstSharedController = source.Share();

            firstSharedController.OpenPin(1);
            source.OpenPin(2);
            firstSharedController.Dispose();
            using var _ = new AssertionScope();

            source.IsPinOpen(1).Should().BeFalse();
            source.IsPinOpen(2).Should().BeTrue();

            firstSharedController.IsPinOpen(1).Should().BeFalse();
            firstSharedController.IsPinOpen(2).Should().BeFalse();
        }
    }
}