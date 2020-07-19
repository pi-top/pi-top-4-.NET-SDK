using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Reactive.Disposables;

using PiTop;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class DigitalPortDeviceBase : IConnectedDevice
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public DigitalPort Port { get; }
        protected IGpioController Controller { get; }

        public ICollection<DisplayPropertyBase> DisplayProperties { get; }

        protected DigitalPortDeviceBase(DigitalPort port, IGpioControllerFactory controllerFactory)
        {
            if (controllerFactory == null)
            {
                throw new ArgumentNullException(nameof(controllerFactory));
            }
            DisplayProperties = new List<DisplayPropertyBase>();
            Port = port;
            Controller = controllerFactory.GetOrCreateController();
            AddToDisposables(Controller);
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

        public void Connect()
        {
            OnInitialise();
        }

        protected virtual void OnInitialise()
        {

        }
    }
}
