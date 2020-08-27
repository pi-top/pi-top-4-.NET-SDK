using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using PiTop.Abstractions;
using Xunit.Sdk;

namespace PiTop.Tests
{
    public class DummyGpioController : IGpioController
    {
        private readonly List<int> _openPins = new List<int>();
        public ICollection<int> OpenPins => _openPins;
        private readonly Dictionary<int, List<PinValue>> _writtenData = new Dictionary<int, List<PinValue>>();
        private readonly Dictionary<int, PinMode> _pinMode = new Dictionary<int, PinMode>();

        public void Dispose()
        {
          
        }

        public PinNumberingScheme NumberingScheme { get; }
        public int PinCount { get; }
        public void OpenPin(int pinNumber)
        {
            OpenPins.Add(pinNumber);
        }

        public void OpenPin(int pinNumber, PinMode mode)
        {
            OpenPins.Add(pinNumber);
            SetPinMode(pinNumber, mode);
        }

        public void ClosePin(int pinNumber)
        {
            OpenPins.Remove(pinNumber);
        }

        public void SetPinMode(int pinNumber, PinMode mode)
        {
            _pinMode[pinNumber] = mode;
        }

        public PinMode GetPinMode(int pinNumber)
        {
            return _pinMode[pinNumber];
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
            if (!_writtenData.TryGetValue(pinNumber, out var data))
            {
                data = new List<PinValue>();
                _writtenData[pinNumber] = data;
            }

            data.Add(value);
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