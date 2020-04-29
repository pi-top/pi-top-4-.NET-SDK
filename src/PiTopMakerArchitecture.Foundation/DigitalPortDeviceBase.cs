using System;
using System.Collections.Generic;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation
{
   
    public abstract class DigitalPortDeviceBase : IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        public DigitalPort Port { get; }

        public ICollection<DisplayPropertyBase> DisplayProperties { get;  }

        protected  DigitalPortDeviceBase(DigitalPort port)
        {
            DisplayProperties = new List<DisplayPropertyBase>();
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
