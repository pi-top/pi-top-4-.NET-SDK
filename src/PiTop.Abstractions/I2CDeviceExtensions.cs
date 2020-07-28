using System;
using System.Device.I2c;

namespace PiTop.Abstractions
{
    public static class I2CDeviceExtensions
    {
        public static void ReadAtRegisterAddress(this I2cDevice device, byte registerAddress, Span<byte> buffer)
        {
            device.WriteByte(registerAddress);
            device.Read(buffer);
        }
    }
}