using System;
using System.Device.Gpio;
using System.Reactive.Disposables;

namespace PiTopMakerArchitecture.Foundation.Sensors
{
    public class Button : DigitalPortDeviceBase
    {
        public event EventHandler<bool> PressedChanged;
        public event EventHandler<EventArgs> Pressed;
        public event EventHandler<EventArgs> Released;
        public Button(DigitalPort port) : base(port)
        {
            var (buttonPin, _) = Port.ToPinPair();
            var controller = new GpioController(PinNumberingScheme.Logical);
            AddToDisposables(Disposable.Create(() =>
            {
                controller.UnregisterCallbackForPinValueChangedEvent(buttonPin, Callback);
                controller.Dispose();
            }));

            controller.OpenPin(buttonPin, PinMode.Input);
            controller.RegisterCallbackForPinValueChangedEvent(buttonPin, PinEventTypes.Falling| PinEventTypes.Rising, Callback);

            IsPressed = controller.Read(buttonPin) == PinValue.Low;
        }

        private void Callback(object _, PinValueChangedEventArgs pinValueChangedEventArgs)
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
