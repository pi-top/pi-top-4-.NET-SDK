using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation
{
    public abstract class AnalogueDeviceBase : PlateConnectedDevice
    {
        public int DeviceAddress { get; }

        public II2CDeviceFactory I2CDeviceFactory { get; }

        protected AnalogueDeviceBase(int deviceAddress, II2CDeviceFactory i2CDeviceFactory)
        {
            DeviceAddress = deviceAddress;
        }
    }
}