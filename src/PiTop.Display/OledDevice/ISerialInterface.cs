using System;

namespace PiTop.OledDevice
{
    public interface ISerialInterface: IDisposable
    {
        void Command(params byte[] cmds);
        void Data(byte data);
    }
}