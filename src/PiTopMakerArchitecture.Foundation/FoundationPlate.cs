using System;
using System.Collections.Generic;
using System.Linq;

using PiTop;

namespace PiTopMakerArchitecture.Foundation
{
    public class FoundationPlate : PiTopPlate
    {
        private readonly DeviceFactory<DigitalPort, DigitalPortDeviceBase> _digitalDeviceFactory;
        private readonly DeviceFactory<AnaloguePort, AnaloguePortDeviceBase> _analogueDeviceFactory;

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
            _digitalDeviceFactory = new DeviceFactory< DigitalPort, DigitalPortDeviceBase>(deviceType =>
            {
                var ctorSignature = new[] { typeof(DigitalPort), typeof(IGpioControllerFactory) };
                var ctor = deviceType.GetConstructor(ctorSignature);
                if (ctor != null)
                {
                    return devicePort => (DigitalPortDeviceBase)Activator.CreateInstance(deviceType, devicePort, module);

                }


                throw new InvalidOperationException(
                    $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
            });

            _analogueDeviceFactory = new DeviceFactory<AnaloguePort, AnaloguePortDeviceBase>(
                deviceType =>
                {
                    var ctorSignature = new[] { typeof(AnaloguePort), typeof(int), typeof(II2CDeviceFactory) };
                    var ctor = deviceType.GetConstructor(ctorSignature);
                    if (ctor != null)
                    {
                        return devicePort =>
                            (AnaloguePortDeviceBase) Activator.CreateInstance(deviceType, devicePort, DefaultI2CAddress, module);

                    }


                    throw new InvalidOperationException(
                        $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
                });

            RegisterForDisposal(_digitalDeviceFactory);
            RegisterForDisposal(_analogueDeviceFactory);
        }

        public IEnumerable<DigitalPortDeviceBase> DigitalDevices => _digitalDeviceFactory.Devices;

        public IEnumerable<AnaloguePortDeviceBase> AnalogueDevices =>
            _analogueDeviceFactory.Devices;

        public override IEnumerable<IPiTopConnectedDevice> Devices => DigitalDevices.Cast<IPiTopConnectedDevice>().Concat(AnalogueDevices);

        public T GetOrCreateDevice<T>(DigitalPort port, Func<DigitalPort, IGpioControllerFactory, T> factory) where T : DigitalPortDeviceBase
        {
            return _digitalDeviceFactory.GetOrCreateDevice<T>(port, (p) => factory(p, Module));
        }

        public T GetOrCreateDevice<T>(DigitalPort port) where T : DigitalPortDeviceBase
        {
            return _digitalDeviceFactory.GetOrCreateDevice<T>(port);
        }

        public T GetOrCreateDevice<T>(AnaloguePort port, Func<AnaloguePort, int, II2CDeviceFactory, T> factory, int deviceAddress = DefaultI2CAddress) where T : AnaloguePortDeviceBase
        {
            return _analogueDeviceFactory.GetOrCreateDevice<T>(port, (p) => factory(p, deviceAddress, Module));
        }

        public T GetOrCreateDevice<T>(AnaloguePort port) where T : AnaloguePortDeviceBase
        {
            return _analogueDeviceFactory.GetOrCreateDevice<T>(port);
        }
    }
}