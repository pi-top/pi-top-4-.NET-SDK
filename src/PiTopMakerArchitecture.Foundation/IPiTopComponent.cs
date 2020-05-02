using System;

namespace PiTopMakerArchitecture.Foundation
{
    public interface IPiTopComponent : IDisposable
    {
        void Initialize();
        
    }
}