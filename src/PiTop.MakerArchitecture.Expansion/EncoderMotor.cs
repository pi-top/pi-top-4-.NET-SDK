using PiTop.Abstractions;
using System;

namespace PiTop.MakerArchitecture.Expansion
{
    public class EncoderMotor : IConnectedDevice
    {
        private readonly SMBusDevice _controller;
        private BrakingType _brakingType;

        public EncoderMotorPort Port { get; }
        private byte REGISTER_MODE_0_POWER { get { return (byte)(0x64 + Port); } }

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

        public ushort Power
        {
            get { return _controller.ReadWord((byte)(REGISTER_MODE_0_POWER)); }
            set { _controller.WriteWord((byte)(REGISTER_MODE_0_POWER), value); }
        }

        public EncoderMotor(EncoderMotorPort port, SMBusDevice controller)
        {
            _controller = controller;
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