using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class DigitalPortDeviceBase : IPiTopComponent
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public DigitalPort Port { get; }
        protected GpioController Controller { get; }

        public ICollection<DisplayPropertyBase> DisplayProperties { get;  }

        protected  DigitalPortDeviceBase(DigitalPort port, GpioController controller)
        {
            DisplayProperties = new List<DisplayPropertyBase>();
            Port = port;
            Controller = controller;
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

        public void Initialize()
        {
            OnInitialise();
        }

        protected virtual void OnInitialise()
        {

        }
    }
}
