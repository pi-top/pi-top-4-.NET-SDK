using System.Device.Gpio;

namespace PiTopMakerArchitecture.Foundation.Components
{
    public class Led : DigitalPortDeviceBase
    {
        private readonly GpioController _controller;
        private readonly int _ledPin;
        private bool _isOn;

        public Led(DigitalPort port) : base(port)
        {
            _controller = new GpioController();
            AddToDisposables(_controller);
            (_ledPin,_) = Port.ToPinPair();
            _controller.OpenPin(_ledPin, PinMode.Output);
        }

        protected override void OnInitialise()
        {
            _isOn = false;
            _controller.Write(_ledPin, PinValue.Low);
        }

        public void On()
        {
            if (!_isOn)
            {
                _isOn = true;
                _controller.Write(_ledPin, PinValue.High);
            }
        }

        public void Off()
        {
            if (_isOn)
            {
                _isOn = false;
                _controller.Write(_ledPin, PinValue.Low);
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