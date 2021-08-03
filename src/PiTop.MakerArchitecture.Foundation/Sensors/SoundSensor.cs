using System;
using UnitsNet;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class SoundSensor : PlateConnectedDevice
    {
        private readonly bool _normalizeValue;
        private AnalogueDigitalConverter _adc;


        public SoundSensor( bool normalizeValue = true) 
        {
            _normalizeValue = normalizeValue;
          
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

        /// <inheritdoc />
        protected override void OnConnection()
        {
            if (Port!.PinPair is { } pinPair)
            {
                var bus = Port.I2CDevice;
                _adc = new AnalogueDigitalConverter(bus, pinPair.pin0);
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }
            
        }
    }
}