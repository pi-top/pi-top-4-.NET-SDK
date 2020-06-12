using System;

namespace PiTop
{
    public interface IConnectedDevice : IDisposable
    {
        void Initialize();
    }
}