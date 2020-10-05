using System;
using System.Collections.Generic;
using System.Linq;

using PiTop;
using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation
{
    public class FoundationPlate : PiTopPlate, IFoundationPlate
    {
        private readonly ConnectedDeviceFactory<DigitalPort, DigitalPortDeviceBase> _digitalConnectedDeviceFactory;
        private readonly ConnectedDeviceFactory<AnaloguePort, AnaloguePortDeviceBase> _analogueConnectedDeviceFactory;

        protected static int DefaultI2CAddress { get; } = 0x04;

        public FoundationPlate(PiTop4Board module) : base(module)
        {
            _digitalConnectedDeviceFactory = new ConnectedDeviceFactory<DigitalPort, DigitalPortDeviceBase>(deviceType =>
           {
               var ctorSignature = new[] { typeof(DigitalPort), typeof(IGpioControllerFactory) };
               var ctor = deviceType.GetConstructor(ctorSignature);
               if (ctor != null)
               {
                   return devicePort => (DigitalPortDeviceBase)Activator.CreateInstance(deviceType, devicePort, module)!;

               }

               throw new InvalidOperationException(
                   $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
           });

            _analogueConnectedDeviceFactory = new ConnectedDeviceFactory<AnaloguePort, AnaloguePortDeviceBase>(
                deviceType =>
                {
                    var ctorSignature = new[] { typeof(AnaloguePort), typeof(int), typeof(II2CDeviceFactory) };
                    var ctor = deviceType.GetConstructor(ctorSignature);
                    if (ctor != null)
                    {
                        return devicePort =>
                            (AnaloguePortDeviceBase)Activator.CreateInstance(deviceType, devicePort, DefaultI2CAddress, module)!;

                    }

                    throw new InvalidOperationException(
                        $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
                });

            RegisterForDisposal(_digitalConnectedDeviceFactory);
            RegisterForDisposal(_analogueConnectedDeviceFactory);
        }

        public IEnumerable<DigitalPortDeviceBase> DigitalDevices => _digitalConnectedDeviceFactory.Devices;

        public IEnumerable<AnaloguePortDeviceBase> AnalogueDevices =>
            _analogueConnectedDeviceFactory.Devices;

        public override IEnumerable<IConnectedDevice> Devices => DigitalDevices.Cast<IConnectedDevice>().Concat(AnalogueDevices);

        public T GetOrCreateDevice<T>(DigitalPort port, Func<DigitalPort, IGpioControllerFactory, T> factory) where T : DigitalPortDeviceBase
        {
            return _digitalConnectedDeviceFactory.GetOrCreateDevice<T>(port, (p) => factory(p, PiTop4Board));
        }

        public T GetOrCreateDevice<T>(DigitalPort port) where T : DigitalPortDeviceBase
        {
            return _digitalConnectedDeviceFactory.GetOrCreateDevice<T>(port);
        }

        public T GetOrCreateDevice<T>(AnaloguePort port, Func<AnaloguePort, int, II2CDeviceFactory, T> factory, int deviceAddress) where T : AnaloguePortDeviceBase
        {
            return _analogueConnectedDeviceFactory.GetOrCreateDevice<T>(port, (p) => factory(p, deviceAddress, PiTop4Board));
        }

        public T GetOrCreateDevice<T>(AnaloguePort port) where T : AnaloguePortDeviceBase
        {
            return _analogueConnectedDeviceFactory.GetOrCreateDevice<T>(port);
        }

        public void DisposeDevice<T>(T device) where T : IConnectedDevice
        {
            switch (device)
            {
                case AnaloguePortDeviceBase analogueDevice:
                    _analogueConnectedDeviceFactory.DisposeDevice(analogueDevice);
                    break;
                case DigitalPortDeviceBase digitalDevice:
                    _digitalConnectedDeviceFactory.DisposeDevice(digitalDevice);
                    break;
            }
        }
    }
}