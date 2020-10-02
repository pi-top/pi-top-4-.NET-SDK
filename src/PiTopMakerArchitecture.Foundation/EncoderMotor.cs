using System;
using System.Device.I2c;
using PiTop.Abstractions;

namespace PiTopMakerArchitecture.Foundation
{
    public enum BrakingType
    {
        Coast = 0,
        Brake
    }
    public class EncoderMotor : IConnectedDevice
    {
        private readonly I2cDevice _bus;
        private BrakingType _brakingType;

        public EncoderMotorPort Port { get; }

        public BrakingType BrakingType
        {
            get => _brakingType;
            set
            {
                SetBrakingType(value);
                _brakingType = value;
            }
        }

        private void SetBrakingType(BrakingType value)
        {
            throw new NotImplementedException();
        }

        public EncoderMotor(EncoderMotorPort port, I2cDevice bus)
        {
            _bus = bus;
            Port = port;
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }
    }
}