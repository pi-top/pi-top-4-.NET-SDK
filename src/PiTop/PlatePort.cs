using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using PiTop.Abstractions;

namespace PiTop
{
    public class PlatePort
    {
        public (int pin0, int pin1)? PinPair { get; }

        public SMBusDevice Bus { get; }

        public GpioController GpioController { get; }

        private PlateConnectedDevice? _device;

        public PlatePort(string name, (int pin0, int pin1)? pinPair = null, GpioController gpioController = null,
            SMBusDevice bus = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
            }

            PinPair = pinPair;
            Bus = bus;
            GpioController = gpioController;
            Name = name;
        }
        

        public PlateConnectedDevice? Device
        {
            get => _device;
            internal set
            {
                if(value is null && _device is {})
                {
                    _device = value;
                }
                else if(_device == value)
                {
                    _device = value;
                }else if (_device is null)
                {
                    _device = value;
                }
                else
                {
                    throw new PlatePortInUseException(this);
                }
            }
        }

        public string Name { get;  }


        public bool Is<T>()
        {
            var enumType = typeof(T);
            if (enumType.IsEnum)
            {
                var names = new HashSet<string>( Enum.GetNames(enumType));
                return names.Contains(Name);
            }

            throw new ArgumentException($"{enumType.Name} is not an enum.");
        }

    }
}