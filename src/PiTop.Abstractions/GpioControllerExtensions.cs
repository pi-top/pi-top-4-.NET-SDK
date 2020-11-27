using System;
using System.Device.Gpio;
using System.Reactive.Disposables;

namespace PiTop.Abstractions
{
    public static class GpioControllerExtensions
    {
        public static IDisposable OpenPinAsDisposable(this GpioController controller, int pinNumber)
        {
            controller.OpenPin(pinNumber);
            return Disposable.Create(() => controller.ClosePin(pinNumber));
        }

        public static IDisposable OpenPinAsDisposable(this GpioController controller, int pinNumber, PinMode mode)
        {
            controller.OpenPin(pinNumber, mode);
            return Disposable.Create(() => controller.ClosePin(pinNumber));
        }

        public static IDisposable RegisterCallbackForPinValueChangedEventAsDisposable(this GpioController controller,
            int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            controller.RegisterCallbackForPinValueChangedEvent(pinNumber, eventTypes, callback);
            return Disposable.Create(() => controller.UnregisterCallbackForPinValueChangedEvent(pinNumber, callback));
        }

        public static GpioController Share(this GpioController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            return new SharedGpioController(controller);
        }
    }
}