using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class AnaloguePortDeviceBase : IPiTopComponent
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public AnaloguePort Port { get; }

        public int DeviceAddress { get; }

        public ICollection<DisplayPropertyBase> DisplayProperties { get; }

        protected AnaloguePortDeviceBase(AnaloguePort port, int deviceAddress)
        {
            DisplayProperties = new List<DisplayPropertyBase>();
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

        public void Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            
        }
    }
}