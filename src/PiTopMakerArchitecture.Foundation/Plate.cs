using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation
{
    public class Plate : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Dictionary<DigitalPort, DigitalPortDeviceBase> _digitalPortDevices = new Dictionary<DigitalPort, DigitalPortDeviceBase>();
        private readonly Dictionary<AnaloguePort, AnaloguePortDeviceBase> _analoguePortDevices = new Dictionary<AnaloguePort, AnaloguePortDeviceBase>();

        private readonly Dictionary<Type, Func<object, object>> _factories = new Dictionary<Type, Func<object, object>>();

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

        public IEnumerable<(DigitalPort port, DigitalPortDeviceBase device)> DigitalDevices =>
            _digitalPortDevices.Select(e => (e.Key, e.Value));

        public IEnumerable<(AnaloguePort port, AnaloguePortDeviceBase device)> AnalogueDevices =>
            _analoguePortDevices.Select(e => (e.Key, e.Value));

        public T GetOrCreateDigitalDevice<T>(DigitalPort port, Func<DigitalPort, T> factory) where T : DigitalPortDeviceBase
        {
            if (_digitalPortDevices.TryGetValue(port, out var digitalDevice) && digitalDevice is T requestedDevice)
            {
                return requestedDevice;
            }

            var newDevice = factory(port);
            _disposables.Add(newDevice);
            _digitalPortDevices[port] = newDevice;
            return newDevice;
        }

        public T GetOrCreateDigitalDevice<T>(DigitalPort port) where T : DigitalPortDeviceBase
        {
            if (!_factories.TryGetValue(typeof(T), out var factory))
            {
                var ctor = typeof(T).GetConstructor(new[] {typeof(DigitalPort)});
                if (ctor != null)
                {
                    factory = (p) => Activator.CreateInstance(typeof(T), p);
                    _factories[typeof(T)] = factory;
                }
            }

            if (factory != null)
            {
                var newDevice = factory.Invoke(port) as T;
                _disposables.Add(newDevice);
                _digitalPortDevices[port] = newDevice;
                return newDevice;
            }
           
            throw new InvalidOperationException();
        }

        public T GetOrCreateAnalogueDevice<T>(AnaloguePort port, Func<AnaloguePort,int, T> factory, int deviceAddress = DefaultI2CAddress) where T : AnaloguePortDeviceBase
        {
            if (_analoguePortDevices.TryGetValue(port, out var analogueDevice) && analogueDevice is T requestedDevice)
            {
                return requestedDevice;
            }

            var newDevice = factory(port, deviceAddress);
            _disposables.Add(newDevice);
            _analoguePortDevices[port] = newDevice;
            return newDevice;
        }

        private class AnalogAddress
        {
            public AnaloguePort Port { get; set; }
            public int Address { get; set; }
        }
        
        public T GetOrCreateAnalogueDevice<T>(AnaloguePort port, int deviceAddress = DefaultI2CAddress) where T : AnaloguePortDeviceBase
        {
            if (!_factories.TryGetValue(typeof(T), out var factory))
            {
                var ctor = typeof(T).GetConstructor(new[] { typeof(AnaloguePort), typeof(int) });
                if (ctor != null)
                {
                    factory = (p) =>
                    {
                        var address = p as AnalogAddress;
                        return Activator.CreateInstance(typeof(T), address.Port, address.Address);
                    };
                    _factories[typeof(T)] = factory;
                }
            }

            if (factory != null)
            {
                var address = new AnalogAddress
                {
                    Port = port,
                    Address = deviceAddress
                };
                var newDevice =  factory.Invoke(address) as T;
                _disposables.Add(newDevice);
                _analoguePortDevices[port] = newDevice;
                return newDevice;
            }

            throw new InvalidOperationException();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}