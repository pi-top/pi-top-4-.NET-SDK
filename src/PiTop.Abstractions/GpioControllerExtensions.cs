using System;
using System.Device.Gpio;

namespace PiTop.Abstractions
{
    public static class GpioControllerExtensions
    {
        public static IGpioController AsManaged(this GpioController controller)
        {
            return new GpioControllerWrapper(controller);
        }

        public static IGpioController Share(this IGpioController controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }
            return new SharedGpioController(controller);
        }


    }
}