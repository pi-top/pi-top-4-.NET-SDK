using System.Device.Gpio;
using System.Reactive.Disposables;
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
        }

        protected override void OnConnection()
        {
            _isOn = false;
            AddToDisposables(Controller.OpenPin(_ledPin, PinMode.Output));
            Controller.Write(_ledPin, PinValue.Low);
        }

        public Led On()
        {
            if (!_isOn)
            {
                _isOn = true;
                Controller.Write(_ledPin, PinValue.High);
            }

            return this;
        }

        public Led Off()
        {
            if (_isOn)
            {
                _isOn = false;
                Controller.Write(_ledPin, PinValue.Low);
            }
            return this;
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

        public Led Toggle()
        {
            IsOn = !IsOn;
            return this;
        }
    }
}