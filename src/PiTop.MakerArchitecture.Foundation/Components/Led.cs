using System;
using System.Device.Gpio;

using PiTop.Abstractions;

namespace PiTop.MakerArchitecture.Foundation.Components
{
    public class Led : PlateConnectedDevice
    {
        private int _ledPin;
        private bool _isOn;
        private GpioController? _controller;


        /// <inheritdoc />
        protected override void OnConnection()
        {
            _controller = Port!.GpioController;
            if (Port!.PinPair is { } pinPair)
            {
                _ledPin = pinPair.pin0;
            }
            else
            {
                throw new InvalidOperationException($"Port {Port.Name} as no pin pair.");
            }

            _isOn = false;
            AddToDisposables(_controller.OpenPinAsDisposable(_ledPin, PinMode.Output));
            _controller.Write(_ledPin, PinValue.Low);
        }
        

        public Led On()
        {
            if (!_isOn && _controller is { })
            {
                _isOn = true;
                _controller.Write(_ledPin, PinValue.High);
            }

            return this;
        }

        public Led Off()
        {
            if (_isOn && _controller is {})
            {
                _isOn = false;
                _controller.Write(_ledPin, PinValue.Low);
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