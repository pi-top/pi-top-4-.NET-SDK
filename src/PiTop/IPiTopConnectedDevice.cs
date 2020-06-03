using System;

namespace PiTop
{
    public interface IPiTopConnectedDevice : IDisposable
    {
        void Initialize();
    }
}