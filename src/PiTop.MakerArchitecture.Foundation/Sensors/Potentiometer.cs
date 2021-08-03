using System;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class Potentiometer : PlateConnectedDevice
    {
        private readonly bool _normalizeValue;
        private AnalogueDigitalConverter _adc;

        
        public Potentiometer( bool normalizeValue = true) 
        {
            _normalizeValue = normalizeValue;
        }

        public double Position => ReadValue();

        private double ReadValue()
        {
            var value = _adc.ReadSample();
            return Math.Round(_normalizeValue ? value / 999.0 : value, 2);
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
