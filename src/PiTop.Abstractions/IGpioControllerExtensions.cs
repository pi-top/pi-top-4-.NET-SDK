using System;
using System.Device.Gpio;
using System.Reactive.Disposables;

namespace PiTop.Abstractions
{
    public static class IGpioControllerExtensions
    {
        public static IDisposable OpenPinAsDisposable(this IGpioController controller, int pinNumber)
        {
            controller.OpenPin(pinNumber);
            return Disposable.Create(() => controller.ClosePin(pinNumber));
        }

        public static  IDisposable OpenPinAsDisposable(this IGpioController controller, int pinNumber, PinMode mode)
        {
            controller.OpenPin(pinNumber, mode);
            return Disposable.Create(() => controller.ClosePin(pinNumber));
        }

        public static IDisposable RegisterCallbackForPinValueChangedEventAsDisposable(this IGpioController controller, int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            controller.RegisterCallbackForPinValueChangedEvent(pinNumber, eventTypes, callback);
            return Disposable.Create(() => controller.UnregisterCallbackForPinValueChangedEvent(pinNumber, callback));
        }

    }
}