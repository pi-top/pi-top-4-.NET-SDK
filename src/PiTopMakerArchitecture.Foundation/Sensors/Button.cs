using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTopMakerArchitecture.Foundation.Sensors
{
    public class Button : DigitalPortDeviceBase
    {
        public event EventHandler<bool>? PressedChanged;
        public event EventHandler<EventArgs>? Pressed;
        public event EventHandler<EventArgs>? Released;
        public Button(DigitalPort port, IGpioControllerFactory controllerFactory) : base(port, controllerFactory)
        {
            var (buttonPin, _) = Port.ToPinPair();
            var openPinAsDisposable = Controller.OpenPinAsDisposable(buttonPin, PinMode.Input);
            var registerCallbackForPinValueChangedEventAsDisposable = Controller.RegisterCallbackForPinValueChangedEventAsDisposable(buttonPin, PinEventTypes.Falling | PinEventTypes.Rising, Callback);

            AddToDisposables(registerCallbackForPinValueChangedEventAsDisposable);
            AddToDisposables(openPinAsDisposable);

            IsPressed = Controller.Read(buttonPin) == PinValue.Low;
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

        public bool IsPressed { get; set; }
    }
}
