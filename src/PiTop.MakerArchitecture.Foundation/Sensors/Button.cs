using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class Button : PlateConnectedDevice
    {
        public event EventHandler<bool>? PressedChanged;
        public event EventHandler<EventArgs>? Pressed;
        public event EventHandler<EventArgs>? Released;

        private GpioController? _controller;


        /// <inheritdoc />
        protected override void OnConnection()
        {
            var buttonPin = -1;
            _controller = Port!.GpioController;
            if (Port!.PinPair is { } pinPair)
            {
                buttonPin = pinPair.pin0;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }
            
            var openPinAsDisposable = _controller.OpenPinAsDisposable(buttonPin, PinMode.Input);
            var registerCallbackForPinValueChangedEventAsDisposable = _controller.RegisterCallbackForPinValueChangedEventAsDisposable(buttonPin, PinEventTypes.Falling | PinEventTypes.Rising, Callback);

            AddToDisposables(registerCallbackForPinValueChangedEventAsDisposable);
            AddToDisposables(openPinAsDisposable);

            IsPressed = _controller.Read(buttonPin) == PinValue.Low;
        }

        private void Callback(object? _, PinValueChangedEventArgs pinValueChangedEventArgs)
        {
            switch (pinValueChangedEventArgs.ChangeType)
            {

                case PinEventTypes.Rising:
                    IsPressed = false;
                    Released?.Invoke(this, EventArgs.Empty);
                    break;
                case PinEventTypes.Falling:
                    IsPressed = true;
                    Pressed?.Invoke(this, EventArgs.Empty);
                    break;

            }
            PressedChanged?.Invoke(this, IsPressed);
        }

        public bool IsPressed { get; private set; }
    }
}
