using System;

namespace PiTop
{
    public class Display : IDisposable
    {
        private readonly OLEDDisplay _oled;


        public Display(IGpioControllerFactory controllerFactory)
        {
          _oled = new OLEDDisplay(controllerFactory);
          
        }

        public void Show()
        {
            _oled.Show();
        }

        public void Hide()
        {
            _oled.Hide();
        }

        public void Reset()
        {
            _oled.Reset();
        }


        public void Dispose()
        {
            _oled.Dispose();
        }

    }
}