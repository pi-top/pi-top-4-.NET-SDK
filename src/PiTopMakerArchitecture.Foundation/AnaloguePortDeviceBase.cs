using System;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class AnaloguePortDeviceBase : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public AnaloguePort Port { get; }

        public int DeviceAddress { get; }

        protected AnaloguePortDeviceBase(AnaloguePort port, int deviceAddress)
        {
            DeviceAddress = deviceAddress;
            Port = port;
        }

        protected void AddToDisposables(IDisposable disposable)
        {
            if (disposable == null)
            {
                throw new ArgumentNullException(nameof(disposable));
            }
            _disposables.Add(disposable);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}