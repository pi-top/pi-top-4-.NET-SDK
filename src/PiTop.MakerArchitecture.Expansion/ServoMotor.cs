using System;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Expansion
{
    public class ServoMotor : IConnectedDevice
    {
        private readonly SMBusDevice _bus;
        public ServoMotorPort Port { get; }

        private byte RegisterControlMode=> (byte) (0x50 + Port);
        private byte RegisterSpeed => (byte)(0x56 + Port);
        private byte RegisterAngleAndSpeed => (byte)(0x5C + Port);

        public ServoMotor(ServoMotorPort port, SMBusDevice bus)
        {
            _bus = bus;
            Port = port;
        }
        public void Dispose()
        {
            Stop();
        }

        private void Stop()
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }
    }
}