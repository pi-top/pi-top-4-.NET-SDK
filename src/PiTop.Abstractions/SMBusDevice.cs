using System;
using System.Device.I2c;
using System.Linq;

namespace PiTop.Abstractions
{
    /// <summary>
    /// Wraps I2C device to expose SMBus functions.
    /// See http://smbus.org/specs/SMBus_3_0_20141220.pdf
    /// </summary>
    public class SMBusDevice
    {
        public I2cDevice I2c { get; }

        public SMBusDevice(I2cDevice device)
        {
            I2c = device;
        }

        public void WriteByte(byte commandCode, byte dataByte)
        {
            I2c.Write(new[] { commandCode, dataByte });
        }


        public byte ReadByte(byte commandCode)
        {
            var data = new byte[1];
            I2c.WriteRead(new[] { commandCode }, data);

            return data[0];
        }

        public void WriteWord(byte commandCode, ushort value)
        {
            I2c.Write(BitConverter.GetBytes(value).Prepend(commandCode).ToArray());
        }

        public ushort ReadWord(byte commandCode)
        {
            var data = new byte[2];

            I2c.WriteRead(new[] { commandCode }, data);

            return BitConverter.ToUInt16(data, 0);
        }

        public ushort ProcessCall(byte commandCode, ushort value)
        {
            var data = new byte[2];

            I2c.WriteRead(BitConverter.GetBytes(value).Prepend(commandCode).ToArray(), data);

            return BitConverter.ToUInt16(data, 0);
        }

        public void WriteBlock(byte commandCode, ReadOnlySpan<byte> values)
        {
            if (values.Length > 32)
            {
                throw new ArgumentOutOfRangeException($"Parameter data is too long (${values.Length} > 32)");
            }

            I2c.Write(values.ToArray().Prepend(Convert.ToByte(values.Length)).Prepend(commandCode).ToArray());
        }

        public ReadOnlySpan<byte> ReadBlock(byte commandCode)
        {
            var data = new byte[32 + 2];
            I2c.WriteRead(new[] { commandCode }, data);

            return data.AsSpan(1, data[0]);
        }
    }
}