using System;
using System.IO;
using FluentAssertions;
using PiTop.Tests;
using Xunit;

namespace PiTop.Camera.Tests
{
    public class CameraExtensionTests   
    {
        [Fact]
        public void cannot_use_factory_if_is_not_initialised()
        {
            PiTopModule.Configure(new DummyGpioController());
            using var module = PiTopModule.Instance;
            var action = new Action(() =>
                module.GetOrCreateCamera<FileSystemCamera>(new DirectoryInfo(Path.GetTempPath()))
            );

            action.Should().Throw<InvalidOperationException>()
                .Which
                .Message
                .Should()
                .Match("Cannot find a factory if type PiTop.Abstractions.IConnectedDeviceFactory<PiTop.Camera.FileSystemCameraSettings, PiTop.Camera.FileSystemCamera>, make sure to configure the module calling UseCamera first.");
        }
    }
}
