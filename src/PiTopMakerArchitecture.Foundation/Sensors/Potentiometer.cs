using System;
using PiTop;

namespace PiTopMakerArchitecture.Foundation.Sensors
{
    public class Potentiometer : AnaloguePortDeviceBase
    {
        private readonly bool _normalizeValue;
        private readonly AnalogueDigitalConverter _adc;

        public Potentiometer(AnaloguePort port, int deviceAddress) : this(port, deviceAddress, true)
        {
        }

        public Potentiometer(AnaloguePort port, int deviceAddress, bool normalizeValue = true) : base(port, deviceAddress)
        {
            _normalizeValue = normalizeValue;
            var (pin1, _) = Port.ToPinPair();
            var bus = PiTopModule.CreateI2CDevice(deviceAddress);
            _adc = new AnalogueDigitalConverter(bus, pin1);

            AddToDisposables(_adc);
        }

        public double Position => ReadValue();

        private double ReadValue()
        {
            var value = _adc.ReadSample();
            return Math.Round(_normalizeValue ? value / 999.0 : value, 2);
        }
    }
}
