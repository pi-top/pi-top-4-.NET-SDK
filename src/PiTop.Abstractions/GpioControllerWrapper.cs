using System;
using System.Device.Gpio;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace PiTop.Abstractions
{
    internal class GpioControllerWrapper : IGpioController
    {
        private readonly GpioController _controller;

        public GpioControllerWrapper(GpioController controller)
        {
            _controller = controller;
        }

        public void Dispose()
        {
            _controller.Dispose();
        }

        public PinNumberingScheme NumberingScheme => _controller.NumberingScheme;

        public int PinCount => _controller.PinCount;

        public IDisposable OpenPin(int pinNumber)
        {
            _controller.OpenPin(pinNumber);
            return Disposable.Create(() => ClosePin(pinNumber));
        }

        public IDisposable OpenPin(int pinNumber, PinMode mode)
        {
            _controller.OpenPin(pinNumber, mode);
            return Disposable.Create(() => ClosePin(pinNumber));
        }

        public void ClosePin(int pinNumber)
        {
            if (_controller.IsPinOpen(pinNumber))
            {
                _controller.ClosePin(pinNumber);
            }
        }

        public void SetPinMode(int pinNumber, PinMode mode)
        {
            _controller.SetPinMode(pinNumber, mode);
        }

        public PinMode GetPinMode(int pinNumber)
        {
            return _controller.GetPinMode(pinNumber);
        }

        public bool IsPinOpen(int pinNumber)
        {
            return _controller.IsPinOpen(pinNumber);
        }

        public bool IsPinModeSupported(int pinNumber, PinMode mode)
        {
            return _controller.IsPinModeSupported(pinNumber, mode);
        }

        public PinValue Read(int pinNumber)
        {
            return _controller.Read(pinNumber);
        }

        public void Write(int pinNumber, PinValue value)
        {
            _controller.Write(pinNumber, value);
        }

        public WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, TimeSpan timeout)
        {
            return _controller.WaitForEvent(pinNumber, eventTypes, timeout);
        }

        public WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken)
        {
            return _controller.WaitForEvent(pinNumber, eventTypes, cancellationToken);
        }

        public ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, TimeSpan timeout)
        {
            return _controller.WaitForEventAsync(pinNumber, eventTypes, timeout);
        }

        public ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, CancellationToken token)
        {
            return _controller.WaitForEventAsync(pinNumber, eventTypes, token);
        }

        public void RegisterCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            _controller.RegisterCallbackForPinValueChangedEvent(pinNumber, eventTypes, callback);
        }

        public void UnregisterCallbackForPinValueChangedEvent(int pinNumber, PinChangeEventHandler callback)
        {
            _controller.UnregisterCallbackForPinValueChangedEvent(pinNumber, callback);
        }

        public void Write(ReadOnlySpan<PinValuePair> pinValuePairs)
        {
            _controller.Write(pinValuePairs);
        }

        public void Read(Span<PinValuePair> pinValuePairs)
        {
            _controller.Read(pinValuePairs);
        }
    }
}