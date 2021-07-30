using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Components
{
    public class Buzzer : PlateConnectedDevice
    {
        private int _buzzPin;
        private bool _isOn;
        private GpioController? _controller;


        public void On()
        {
            if (!_isOn && _controller is { })
            {
                _isOn = true;
                _controller.Write(_buzzPin, PinValue.High);
            }
        }

        public void Off()
        {
            if (_isOn && _controller is { })
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

        /// <inheritdoc />
        protected override void OnConnection()
        {
            _controller = Port!.GpioController;
            if (Port!.PinPair is { } pinPair)
            {
                _buzzPin = pinPair.pin0;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }
            
            AddToDisposables(_controller.OpenPinAsDisposable(_buzzPin, PinMode.Output));
        }
    }
}