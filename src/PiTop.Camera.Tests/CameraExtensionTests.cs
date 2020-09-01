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
            PiTop4Board.Configure(new DummyGpioController());
            using var module = PiTop4Board.Instance;
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
