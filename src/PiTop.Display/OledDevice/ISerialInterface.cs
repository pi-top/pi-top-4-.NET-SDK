using System;

namespace PiTop.OledDevice
{
    public interface ISerialInterface: IDisposable
    {
        void Command(byte cmd);
        void Data(byte data);
    }
}