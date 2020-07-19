using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace PiTop.Tests
{
    internal class DummyGpioController : IGpioController
    {
        private readonly List<int> _openPins = new List<int>();
        public ICollection<int> OpenPins => _openPins;

        public void Dispose()
        {
          
        }

        public PinNumberingScheme NumberingScheme { get; }
        public int PinCount { get; }
        public IDisposable OpenPin(int pinNumber)
        {
            OpenPins.Add(pinNumber);
            return Disposable.Create(() => ClosePin(pinNumber));
        }

        public IDisposable OpenPin(int pinNumber, PinMode mode)
        {
            OpenPins.Add(pinNumber);
            return Disposable.Create(() => ClosePin(pinNumber));
        }

        public void ClosePin(int pinNumber)
        {
            OpenPins.Remove(pinNumber);
        }

        public void SetPinMode(int pinNumber, PinMode mode)
        {
            
        }

        public PinMode GetPinMode(int pinNumber)
        {
            throw new NotImplementedException();
        }

        public bool IsPinOpen(int pinNumber)
        {
            return OpenPins.Contains(pinNumber);
        }

        public bool IsPinModeSupported(int pinNumber, PinMode mode)
        {
            throw new NotImplementedException();
        }

        public PinValue Read(int pinNumber)
        {
            throw new NotImplementedException();
        }

        public void Write(int pinNumber, PinValue value)
        {
            throw new NotImplementedException();
        }

        public WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        public void RegisterCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            throw new NotImplementedException();
        }

        public void UnregisterCallbackForPinValueChangedEvent(int pinNumber, PinChangeEventHandler callback)
        {
            throw new NotImplementedException();
        }

        public void Write(ReadOnlySpan<PinValuePair> pinValuePairs)
        {
            throw new NotImplementedException();
        }

        public void Read(Span<PinValuePair> pinValuePairs)
        {
            throw new NotImplementedException();
        }
    }
}