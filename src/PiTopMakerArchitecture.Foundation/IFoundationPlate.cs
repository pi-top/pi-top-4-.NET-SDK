using System;
using System.Collections.Generic;

using PiTop.Abstractions;

namespace PiTopMakerArchitecture.Foundation
{
    public interface IFoundationPlate
    {
        IEnumerable<DigitalPortDeviceBase> DigitalDevices { get; }
        IEnumerable<AnaloguePortDeviceBase> AnalogueDevices { get; }
        IEnumerable<IConnectedDevice> Devices { get; }
        T GetOrCreateDevice<T>(DigitalPort port, Func<DigitalPort, IGpioControllerFactory, T> factory) where T : DigitalPortDeviceBase;
        T GetOrCreateDevice<T>(DigitalPort port) where T : DigitalPortDeviceBase;
        T GetOrCreateDevice<T>(AnaloguePort port, Func<AnaloguePort, int, II2CDeviceFactory, T> factory, int deviceAddress) where T : AnaloguePortDeviceBase;
        T GetOrCreateDevice<T>(AnaloguePort port) where T : AnaloguePortDeviceBase;
        void DisposeDevice<T>(T device) where T : IConnectedDevice;
    }
}