using System;
using System.Device.I2c;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Expansion
{
    public abstract class ServoMotor : IConnectedDevice
    {
        private readonly I2cDevice _bus;
        public ServoMotorPort Port { get; }

        public ServoMotor(ServoMotorPort port, I2cDevice bus)
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