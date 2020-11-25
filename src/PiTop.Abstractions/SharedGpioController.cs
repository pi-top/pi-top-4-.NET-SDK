using System.Collections.Generic;
using System.Device.Gpio;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PiTop.Abstractions
{
    internal class SharedGpioController : GpioController
    {
        private readonly List<int> _openPins = new List<int>();
        private readonly GpioController _controller;

        public SharedGpioController(GpioController controller) : base(controller.NumberingScheme, null)
        {
            _controller = controller;
        }

        protected override void OpenPinCore(int pinNumber)
        {
            try
            {
                _controller.OpenPin(pinNumber);
                _openPins.Add(pinNumber);
            }
            catch (IOException ex)
            {
                throw new IOException($"Error opening pin {pinNumber}: " + ex.Message, ex);
            }
        }

        protected override void ClosePinCore(int pinNumber)
        {
            if (_openPins.Remove(pinNumber))
            {
                _controller.ClosePin(pinNumber);
            }
        }

        public override void SetPinMode(int pinNumber, PinMode mode)
        {
            _controller.SetPinMode(pinNumber, mode);
        }

        public override PinMode GetPinMode(int pinNumber)
        {
            return _controller.GetPinMode(pinNumber);
        }

        public override bool IsPinModeSupported(int pinNumber, PinMode mode)
        {
            return _openPins.Contains(pinNumber);
        }

        public override PinValue Read(int pinNumber)
        {
            return _controller.Read(pinNumber);
        }

        public override void Write(int pinNumber, PinValue value)
        {
            _controller.Write(pinNumber, value);
        }

        public override WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken)
        {
            return _controller.WaitForEvent(pinNumber, eventTypes, cancellationToken);
        }

        public override ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, CancellationToken token)
        {
            return _controller.WaitForEventAsync(pinNumber, eventTypes, token);
        }

        public override void RegisterCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            _controller.RegisterCallbackForPinValueChangedEvent(pinNumber, eventTypes, callback);
        }

        public override void UnregisterCallbackForPinValueChangedEvent(int pinNumber, PinChangeEventHandler callback)
        {
            _controller.UnregisterCallbackForPinValueChangedEvent(pinNumber, callback);
        }

        protected override void Dispose(bool disposing)
        {
            foreach (var openPin in _openPins)
            {
                _controller.ClosePin(openPin);
            }
            _openPins.Clear();
        }

        public override int PinCount => _controller.PinCount;
    }
}