using PiTop.Abstractions;

using System;

namespace PiTop.MakerArchitecture.Expansion
{
    public class EncoderMotor : IConnectedDevice
    {
        private readonly SMBusDevice _controller;
        private BrakingType _brakingType;

        public EncoderMotorPort Port { get; }
        private byte RegisterMode0Power => (byte)(0x64 + Port);

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
            get => _controller.ReadWord(RegisterMode0Power);
            set => _controller.WriteWord(RegisterMode0Power, value);
        }

        public EncoderMotor(EncoderMotorPort port, SMBusDevice controller)
        {
            _controller = controller;
            Port = port;
        }
        public void Dispose()
        {
            Power = 0;
        }

        public void Connect()
        {
            Power = 0;
        }
    }
}