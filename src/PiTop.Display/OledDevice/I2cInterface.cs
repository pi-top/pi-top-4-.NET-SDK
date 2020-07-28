using System;
using System.Device.I2c;

namespace PiTop.OledDevice
{
    internal class I2cInterface : ISerialInterface
    {
        private readonly I2cDevice _device;


        public I2cInterface(I2cDevice device)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Command(byte cmd){
            throw new NotImplementedException();
        }
        
        public void Data(byte data){
            throw new NotImplementedException();
        }     
    }
}