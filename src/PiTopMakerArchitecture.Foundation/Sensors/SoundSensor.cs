using System;
using PiTop;

namespace PiTopMakerArchitecture.Foundation.Sensors
{
    public class SoundSensor : AnaloguePortDeviceBase
    {
        private readonly bool _normalizeValue;
        private readonly AnalogueDigitalConverter _adc;

        public SoundSensor(AnaloguePort port, int deviceAddress) : this(port, deviceAddress, true)
        {
        }

        public SoundSensor(AnaloguePort port, int deviceAddress, bool normalizeValue = true) : base(port, deviceAddress)
        {
            _normalizeValue = normalizeValue;
            var (pin1, _) = Port.ToPinPair();
            var bus = PiTopModule.CreateI2CDevice(deviceAddress);
            _adc = new AnalogueDigitalConverter(bus, pin1);

            AddToDisposables(_adc);
        }

        public double Value => ReadValue();

        private double ReadValue()
        {
            var value = _adc.ReadSample(peakDetection:true) / 2.0;
            return Math.Round(value);
        }
    }
}