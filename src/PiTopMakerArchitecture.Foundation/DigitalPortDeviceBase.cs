using System;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class DigitalPortDeviceBase : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public DigitalPort Port { get; }

        protected  DigitalPortDeviceBase(DigitalPort port)
        {
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
