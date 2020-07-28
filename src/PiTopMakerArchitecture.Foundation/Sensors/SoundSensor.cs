using System;

using PiTop;
using PiTop.Abstractions;
using UnitsNet;

namespace PiTopMakerArchitecture.Foundation.Sensors
{
    public class SoundSensor : AnaloguePortDeviceBase
    {
        private readonly bool _normalizeValue;
        private readonly AnalogueDigitalConverter _adc;

        public SoundSensor(AnaloguePort port, int deviceAddress, II2CDeviceFactory i2CDeviceFactory) : this(port, deviceAddress, i2CDeviceFactory, true)
        {
        }

        public SoundSensor(AnaloguePort port, int deviceAddress, II2CDeviceFactory i2CDeviceFactory, bool normalizeValue) : base(port, deviceAddress, i2CDeviceFactory)
        {
            _normalizeValue = normalizeValue;
            var (pin1, _) = Port.ToPinPair();
            var bus = i2CDeviceFactory.GetOrCreateI2CDevice(DeviceAddress);
            _adc = new AnalogueDigitalConverter(bus, pin1);

            AddToDisposables(_adc);
        }

        public double Value => ReadValue(_normalizeValue);

        public Level Level
        {
            get
            {
                var db = 20 * Math.Log10(ReadValue(true));
                return Level.FromDecibels(db);
            }
        }

        private double ReadValue(bool normalize)
        {
            var value = _adc.ReadSample(peakDetection: true);
            if (normalize)
            {
                value /= ushort.MaxValue;
            }
            return Math.Round(value);
        }
    }
}