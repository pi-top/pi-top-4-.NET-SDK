using System.Device.Gpio;

namespace PiTopMakerArchitecture.Foundation.Components
{
    public class Buzzer : DigitalPortDeviceBase
    {
        private readonly GpioController _controller;
        private readonly int _buzzPin;
        private bool _isOn;

        public Buzzer(DigitalPort port) : base(port)
        {
            _controller = new GpioController();
            AddToDisposables(_controller);
            (_buzzPin, _) = Port.ToPinPair();
            _controller.OpenPin(_buzzPin, PinMode.Output);
        }

        public void On()
        {
            if (!_isOn)
            {
                _isOn = true;
                _controller.Write(_buzzPin, PinValue.High);
            }
        }

        public void Off()
        {
            if (_isOn)
            {
                _isOn = false;
                _controller.Write(_buzzPin, PinValue.Low);
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