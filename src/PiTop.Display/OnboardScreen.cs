using System;

namespace PiTop.Display
{
    public class OnboardScreen : IDisposable
    {
        private readonly OLEDDisplay _device;


        public OnboardScreen(IGpioControllerFactory controllerFactory)
        {
          _device = new OLEDDisplay(controllerFactory);
          
        }

        public void Show()
        {
            _device.Show();
        }

        public void Hide()
        {
            _device.Hide();
        }

        public void Reset()
        {
            _device.Reset();
        }


        public void Dispose()
        {
            _device.Dispose();
        }

    }
}