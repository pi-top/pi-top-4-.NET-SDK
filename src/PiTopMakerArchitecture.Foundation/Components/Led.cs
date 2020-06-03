using System.Device.Gpio;

using PiTop;

namespace PiTopMakerArchitecture.Foundation.Components
{
    public class Led : DigitalPortDeviceBase
    {
        private readonly int _ledPin;
        private bool _isOn;

        public Led(DigitalPort port, IGpioControllerFactory controllerFactory) : base(port, controllerFactory)
        {
            (_ledPin, _) = Port.ToPinPair();
            Controller.OpenPin(_ledPin, PinMode.Output);
        }

        protected override void OnInitialise()
        {
            _isOn = false;
            Controller.Write(_ledPin, PinValue.Low);
        }

        public void On()
        {
            if (!_isOn)
            {
                _isOn = true;
                Controller.Write(_ledPin, PinValue.High);
            }
        }

        public void Off()
        {
            if (_isOn)
            {
                _isOn = false;
                Controller.Write(_ledPin, PinValue.Low);
            }
        }

        public bool IsOn
        {
            get => _isOn;
            set
            {
                if (value)
                {
                    On();
                }
                else
                {
                    Off();
                }
            }
        }

        public void Toggle()
        {
            IsOn = !IsOn;
        }
    }
}