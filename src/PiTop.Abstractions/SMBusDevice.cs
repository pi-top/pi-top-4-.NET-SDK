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
        private readonly I2cDevice _device;

        public SMBusDevice(I2cDevice device)
        {
            _device = device;
        }

        public void WriteByte(byte commandCode, byte dataByte)
        {
            _device.Write(new[] { commandCode, dataByte });
        }


        public byte ReadByte(byte commandCode)
        {
            var data = new byte[1];
            _device.WriteRead(new[] { commandCode }, data);

            return data[0];
        }

        public void WriteWord(byte commandCode, ushort value)
        {
            _device.Write(BitConverter.GetBytes(value).Prepend(commandCode).ToArray());
        }

        public ushort ReadWord(byte commandCode)
        {
            var data = new byte[2];

            _device.WriteRead(new[] { commandCode }, data);

            return BitConverter.ToUInt16(data, 0);
        }

        public ushort ProcessCall(byte commandCode, ushort value)
        {
            var data = new byte[2];

            _device.WriteRead(BitConverter.GetBytes(value).Prepend(commandCode).ToArray(), data);

            return BitConverter.ToUInt16(data, 0);
        }

        public void WriteBlock(byte commandCode, ReadOnlySpan<byte> values)
        {
            if (values.Length > 32)
            {
                throw new ArgumentOutOfRangeException($"Parameter data is too long (${values.Length} > 32)");
            }

            _device.Write(values.ToArray().Prepend(Convert.ToByte(values.Length)).Prepend(commandCode).ToArray());
        }


        public ReadOnlySpan<byte> ReadBlock(byte commandCode)
        {
            var data = new byte[32 + 2];
            _device.WriteRead(new[] { commandCode }, data);

            return data.AsSpan(1, data[0]);
        }
    }
}