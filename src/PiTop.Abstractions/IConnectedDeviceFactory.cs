using System;
using System.Collections.Generic;

namespace PiTop.Abstractions
{
    public interface IConnectedDeviceFactory<TConnectionConfiguration, TDevice> : IDisposable
        where TConnectionConfiguration : notnull
        where TDevice : IConnectedDevice
    {
        T GetOrCreateDevice<T>(TConnectionConfiguration connectionConfiguration) where T : TDevice;
        T GetOrCreateDevice<T>(TConnectionConfiguration connectionConfiguration, Func<TConnectionConfiguration, TDevice> deviceFactory) where T : TDevice;
        IEnumerable<TDevice> Devices { get;  }

        void DisposeDevice<T>(T device) where T : TDevice;
    }
}