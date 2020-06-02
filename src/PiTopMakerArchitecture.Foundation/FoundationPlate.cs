using System;
using System.Collections.Generic;
using System.Linq;
using PiTop;

namespace PiTopMakerArchitecture.Foundation
{
    public class FoundationPlate : PiTopPlate
    {
        private readonly Dictionary<DigitalPort, DigitalPortDeviceBase> _digitalPortDevices = new Dictionary<DigitalPort, DigitalPortDeviceBase>();
        private readonly Dictionary<AnaloguePort, AnaloguePortDeviceBase> _analoguePortDevices = new Dictionary<AnaloguePort, AnaloguePortDeviceBase>();
        private readonly Dictionary<Type, Func<object, object, object>> _factories = new Dictionary<Type, Func<object, object, object>>();

        //PMA Foundation for RPI I2C Registers
        //0x10 ~ 0x17: ADC raw data
        //0x20 ~ 0x27: input voltage
        //0x29: output voltage (PMA power supply voltage)
        //0x30 ~ 0x37: input voltage / output voltage
        //0x18 ~ 0x1F: peak detection

        private const int DefaultI2CAddress = 0x04;


        private const int RpiFoundationPid = 0x0004;
        private const int RpiZeroFoundationPid = 0x0005;
        private const string RpiFoundationName = "PI-TOP Foundation Base RPI";
        private const string RpiZeroFoundationName = "PI-TOP Foundation Base  RPi Zero";

        public FoundationPlate(PiTopModule module) : base(module)
        {
        }

        public IEnumerable<DigitalPortDeviceBase> DigitalDevices =>
            _digitalPortDevices.Select(e => ( e.Value));

        public IEnumerable<AnaloguePortDeviceBase> AnalogueDevices =>
            _analoguePortDevices.Select(e => e.Value);

        public T GetOrCreateDigitalDevice<T>(DigitalPort port, Func<DigitalPort, IGpioControllerFactory,  T> factory) where T : DigitalPortDeviceBase
        {
            if (_digitalPortDevices.TryGetValue(port, out var digitalDevice) && digitalDevice is T requestedDevice)
            {
                return requestedDevice;
            }

            var newDevice = factory(port, Module);
            RegisterForDisposal(newDevice);
            _digitalPortDevices[port] = newDevice;
            newDevice.Initialize();
            return newDevice;
        }

        public T GetOrCreateDigitalDevice<T>(DigitalPort port) where T : DigitalPortDeviceBase
        {
            if (_digitalPortDevices.TryGetValue(port, out var digitalDevice) && digitalDevice is T requestedDevice)
            {
                return requestedDevice;
            }

            if (!_factories.TryGetValue(typeof(T), out var factory))
            {
                var ctor = typeof(T).GetConstructor(new[] {typeof(DigitalPort), typeof(IGpioControllerFactory)});
                if (ctor != null)
                {
                    factory = (p,c) => Activator.CreateInstance(typeof(T), p, c);
                    _factories[typeof(T)] = factory;
                }
            }

            if (factory != null)
            {
                return GetOrCreateDigitalDevice(port, (dp, c) => factory(dp,c) as T);
            }
           
            throw new InvalidOperationException();
        }

        public T GetOrCreateAnalogueDevice<T>(AnaloguePort port, Func<AnaloguePort,int, II2CDeviceFactory, T> factory, int deviceAddress = DefaultI2CAddress) where T : AnaloguePortDeviceBase
        {
            if (_analoguePortDevices.TryGetValue(port, out var analogueDevice) && analogueDevice is T requestedDevice)
            {
                return requestedDevice;
            }

            var newDevice = factory(port, deviceAddress, Module);
            RegisterForDisposal(newDevice);
            _analoguePortDevices[port] = newDevice;
            newDevice.Initialize();
            return newDevice;
        }

        private class AnalogAddress
        {
            public AnaloguePort Port { get; set; }
            public int Address { get; set; }
        }
        
        public T GetOrCreateAnalogueDevice<T>(AnaloguePort port, int deviceAddress = DefaultI2CAddress) where T : AnaloguePortDeviceBase
        {
            if (_analoguePortDevices.TryGetValue(port, out var analogueDevice) && analogueDevice is T requestedDevice)
            {
                return requestedDevice;
            }

            if (!_factories.TryGetValue(typeof(T), out var factory))
            {
                var ctor = typeof(T).GetConstructor(new[] { typeof(AnaloguePort), typeof(int), typeof(II2CDeviceFactory) });
                if (ctor != null)
                {
                    factory = (p,bf) =>
                    {
                        var address = p as AnalogAddress;
                        return Activator.CreateInstance(typeof(T), address.Port, address.Address, bf);
                    };
                    _factories[typeof(T)] = factory;
                }
            }

            if (factory != null)
            {

                return GetOrCreateAnalogueDevice(port, (ap,da, bf) => factory(new AnalogAddress{Port = ap, Address = da}, bf) as T, deviceAddress);
            }

            throw new InvalidOperationException();
        }

    }
}