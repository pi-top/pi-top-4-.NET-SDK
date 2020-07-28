using System.Device.Gpio;
using System.Reactive.Disposables;
using PiTop;
using PiTop.Abstractions;

namespace PiTopMakerArchitecture.Foundation.Components
{
    public class Buzzer : DigitalPortDeviceBase
    {
        private readonly int _buzzPin;
        private bool _isOn;

        public Buzzer(DigitalPort port, IGpioControllerFactory controllerFactory) : base(port, controllerFactory)
        {
            (_buzzPin, _) = Port.ToPinPair();
            AddToDisposables(Controller.OpenPin(_buzzPin, PinMode.Output));
        }

        public void On()
        {
            if (!_isOn)
            {
                _isOn = true;
                Controller.Write(_buzzPin, PinValue.High);
            }
        }

        public void Off()
        {
            if (_isOn)
            {
                _isOn = false;
                Controller.Write(_buzzPin, PinValue.Low);
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