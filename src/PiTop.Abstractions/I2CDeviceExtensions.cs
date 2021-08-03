using System;
using System.Device.I2c;
using System.Linq;

namespace PiTop.Abstractions
{
    public static class I2CDeviceExtensions
    {
        public static void ReadAtRegisterAddress(this I2cDevice device, byte registerAddress, Span<byte> buffer)
        {
            device.WriteByte(registerAddress);
            device.Read(buffer);
        }


        public static void WriteByte(this I2cDevice device, byte commandCode, byte dataByte)
        {
            device.Write(new[] {commandCode, dataByte});
        }


        public static byte ReadByte(this I2cDevice device, byte commandCode)
        {
            var data = new byte[1];
            device.WriteRead(new[] {commandCode}, data);

            return data[0];
        }

        public static void WriteWord(this I2cDevice device, byte commandCode, ushort value)
        {
            device.Write(BitConverter.GetBytes(value).Prepend(commandCode).ToArray());
        }

        public static void WriteWord(this I2cDevice device, byte commandCode, short value)
        {
            device.Write(BitConverter.GetBytes(value).Prepend(commandCode).ToArray());
        }

        public static ushort ReadWord(this I2cDevice device, byte commandCode)
        {
            var data = new byte[2];

            device.WriteRead(new[] {commandCode}, data);

            return BitConverter.ToUInt16(data, 0);
        }

        public static short ReadWordSigned(this I2cDevice device, byte commandCode)
        {
            var data = new byte[2];

            device.WriteRead(new[] {commandCode}, data);

            return BitConverter.ToInt16(data, 0);
        }


        public static ushort ProcessCall(this I2cDevice device, byte commandCode, ushort value)
        {
            var data = new byte[2];

            device.WriteRead(BitConverter.GetBytes(value).Prepend(commandCode).ToArray(), data);

            return BitConverter.ToUInt16(data, 0);
        }

        public static void WriteBlock(this I2cDevice device, byte commandCode, ReadOnlySpan<byte> values)
        {
            if (values.Length > 32)
            {
                throw new ArgumentOutOfRangeException($"Parameter data is too long (${values.Length} > 32)");
            }

            var data = values.ToArray().Prepend(Convert.ToByte(values.Length)).Prepend(commandCode).ToArray();
            device.Write(data);
        }

        public static ReadOnlySpan<byte> ReadBlock(this I2cDevice device, byte commandCode)
        {
            var data = new byte[32 + 2];
            device.WriteRead(new[] {commandCode}, data);

            return data.AsSpan(1, data[0]);
        }

        public static void Write32(this I2cDevice device, byte commandCode, short b1, short b2)
        {
            device.Write32(commandCode, BitConverter.GetBytes(b1).Concat(BitConverter.GetBytes(b2)).ToArray());
        }

        public static void Write32(this I2cDevice device, byte commandCode, ReadOnlySpan<byte> data)
        {
            if (data.Length != 4)
            {
                throw new ArgumentOutOfRangeException($"Parameter data is needs to be length 4 (${data.Length} > 32)");
            }

            device.Write(data.ToArray().Prepend(commandCode).ToArray());
        }

        public static ReadOnlySpan<byte> Read32(this I2cDevice device, byte commandCode)
        {
            var data = new byte[4];
            device.WriteRead(new[] {commandCode}, data);
            return data;
        }
    }
}