using System.Drawing;
using System.IO;
using FluentAssertions;
using PiTop.Tests;
using Xunit;

namespace PiTop.Camera.Tests
{
    public class FileSystemCameraTests
    {
        [Fact]
        public void can_be_created_via_factory()
        {
            
            using var module = new PiTopModule(new DummyGpioController());
            module.UseCamera();
            using var camera = module.GetOrCreateCamera<FileSystemCamera>(new DirectoryInfo(Path.GetTempPath()));

            camera.Should()
                .NotBeNull();
        }

        [Fact]
        public void can_load_images()
        {
            using var dir = DisposableDirectory.CreateTemp();
            using var module = new PiTopModule(new DummyGpioController());
            module.UseCamera();
            using var camera = module.GetOrCreateCamera<FileSystemCamera>(dir.Root);

            camera.FrameCount.Should()
                .Be(3);
        }

        [Fact]
        public void can_scan_images()
        {
            using var dir = DisposableDirectory.CreateTemp();
            using var module = new PiTopModule(new DummyGpioController());
            module.UseCamera();
            using var camera = module.GetOrCreateCamera<FileSystemCamera>(dir.Root);

            var source1 = camera.CurrentFrameSource;
            camera.Advance();
            var source2 = camera.CurrentFrameSource;
            source2.Name.Should().NotBe(source1.Name);

        }
    }
}