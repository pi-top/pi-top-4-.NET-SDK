using System.Device.Spi;

namespace PiTop.Abstractions
{
    public interface ISPiDeviceFactory
    {
        SpiDevice GetOrCreateSpiDevice(SpiConnectionSettings connectionSettings);
    }
}