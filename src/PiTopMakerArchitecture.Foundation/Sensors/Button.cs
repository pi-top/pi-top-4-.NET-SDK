using System;
using System.Device.Gpio;
using System.Reactive.Disposables;

using PiTop;

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

            AddToDisposables(Disposable.Create(() =>
            {
                Controller.UnregisterCallbackForPinValueChangedEvent(buttonPin, Callback);
            }));

            AddToDisposables(Controller.OpenPin(buttonPin, PinMode.Input));
            Controller.RegisterCallbackForPinValueChangedEvent(buttonPin, PinEventTypes.Falling | PinEventTypes.Rising, Callback);

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
