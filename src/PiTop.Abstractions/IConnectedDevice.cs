using System;

namespace PiTop.Abstractions
{
    public interface IConnectedDevice : IDisposable
    {
        void Connect();
    }
}