using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

using PiTop;
using PiTop.Abstractions;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class AnaloguePortDeviceBase : IConnectedDevice
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public AnaloguePort Port { get; }

        public int DeviceAddress { get; }
        public II2CDeviceFactory I2CDeviceFactory { get; }

        public ICollection<DisplayPropertyBase> DisplayProperties { get; }

        protected AnaloguePortDeviceBase(AnaloguePort port, int deviceAddress, II2CDeviceFactory i2CDeviceFactory)
        {
            DisplayProperties = new List<DisplayPropertyBase>();
            DeviceAddress = deviceAddress;
            I2CDeviceFactory = i2CDeviceFactory;
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

        public void Connect()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {

        }
    }
}