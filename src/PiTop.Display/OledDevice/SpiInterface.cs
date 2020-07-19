using System;
using System.Device.Gpio;
using System.Device.Spi;
using System.Reactive.Disposables;

namespace PiTop.OledDevice
{
    internal class SpiInterface : ISerialInterface
    {
        private readonly SpiDevice _device;
        private readonly IGpioController _controller;
        private readonly int _dcPin;
        private readonly int _rstPin;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public SpiInterface(SpiDevice device, IGpioController controller, int dcPin, int rstPin)
        {
            _device = device ?? throw new ArgumentNullException(nameof(device));
            _controller = controller ?? throw new ArgumentNullException(nameof(controller));
            _dcPin = dcPin;
            _rstPin = rstPin;

            _disposables.Add( _controller.OpenPin(_dcPin, PinMode.Output));
            _disposables.Add(_controller.OpenPin(_rstPin, PinMode.Output));

            _controller.Write(_rstPin, PinValue.Low);
            _controller.Write(_rstPin, PinValue.High);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}