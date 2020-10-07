using System;
using System.Device.I2c;

namespace PiTop.Tests
{
    public class DummyI2CDevice : I2cDevice
    {
        public DummyI2CDevice(I2cConnectionSettings connectionSettings)
        {
            ConnectionSettings = connectionSettings;
        }

        public override byte ReadByte()
        {
            return 0;
        }

        public override void Read(Span<byte> buffer)
        {
           
        }

        public override void WriteByte(byte value)
        {
          
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            
        }

        public override void WriteRead(ReadOnlySpan<byte> writeBuffer, Span<byte> readBuffer)
        {
            
        }

        public override I2cConnectionSettings ConnectionSettings { get; }
    }
}