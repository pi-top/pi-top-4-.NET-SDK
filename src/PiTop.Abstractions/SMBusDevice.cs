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

        public void WriteWord(byte commandCode, short value)
        {
            I2c.Write(BitConverter.GetBytes(value).Prepend(commandCode).ToArray());
        }

        public ushort ReadWord(byte commandCode)
        {
            var data = new byte[2];

            I2c.WriteRead(new[] { commandCode }, data);

            return BitConverter.ToUInt16(data, 0);
        }

        public short ReadWordSigned(byte commandCode)
        {
            var data = new byte[2];

            I2c.WriteRead(new[] { commandCode }, data);

            return BitConverter.ToInt16(data, 0);
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
            var data = values.ToArray().Prepend(Convert.ToByte(values.Length)).Prepend(commandCode).ToArray();
            I2c.Write(data);
        }

        public ReadOnlySpan<byte> ReadBlock(byte commandCode)
        {
            var data = new byte[32 + 2];
            I2c.WriteRead(new[] { commandCode }, data);

            return data.AsSpan(1, data[0]);
        }

        public void Write32(byte commandCode, short b1, short b2)
        {
            Write32(commandCode, BitConverter.GetBytes(b1).Concat(BitConverter.GetBytes(b2)).ToArray());
        }

        public void Write32(byte commandCode, ReadOnlySpan<byte> data)
        {
            if (data.Length != 4)
            {
                throw new ArgumentOutOfRangeException($"Parameter data is needs to be length 4 (${data.Length} > 32)");
            }
            I2c.Write(data.ToArray().Prepend(commandCode).ToArray());
        }

        public ReadOnlySpan<byte> Read32(byte commandCode)
        {
            var data = new byte[4];
            I2c.WriteRead(new[] { commandCode }, data);
            return data;
        }
    }
}