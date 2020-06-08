using System;
using System.Collections.Generic;

namespace PiTop
{
    public interface IDeviceFactory<TPort, TDevice>
        where TDevice : IPiTopConnectedDevice
    {
        T GetOrCreateDevice<T>(TPort port) where T : TDevice;
        T GetOrCreateDevice<T>(TPort port, Func<TPort, TDevice> deviceFactory) where T : TDevice;
        IEnumerable<TDevice> Devices { get;  }
    }
}