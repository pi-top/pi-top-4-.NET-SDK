using System;
using System.Linq;
using FluentAssertions;
using PiTop.Abstractions;
using Xunit;

namespace PiTop.Tests
{
    public class SimpleDevice : IConnectedDevice
    {
        public int Id { get; }

        public SimpleDevice(int id)
        {
            Id = id;
        }
        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        public void Connect()
        {

        }
    }
    public class DeviceFactoryTests
    {
        [Fact]
        public void using_again_the_same_connection_settings_return_the_same_device_instance()
        {
            var factory = new ConnectedDeviceFactory<int, SimpleDevice>(type =>
            {
                return i => (SimpleDevice)Activator.CreateInstance(type, i);
            });

            var firstDevice = factory.GetOrCreateDevice<SimpleDevice>(1);
            var secondDevice = factory.GetOrCreateDevice<SimpleDevice>(1);

            firstDevice.Should().BeSameAs(secondDevice);
        }

        [Fact]
        public void disposing_the_factory_disposes_all_devices()
        {
            var factory = new ConnectedDeviceFactory<int, SimpleDevice>(type =>
            {
                return i => (SimpleDevice)Activator.CreateInstance(type, i);
            });

            var devices = Enumerable.Range(0, 10).Select(factory.GetOrCreateDevice<SimpleDevice>).ToList();

            factory.Dispose();

            devices.Should().NotContain(device => device.IsDisposed == false);
        }

    }
}
