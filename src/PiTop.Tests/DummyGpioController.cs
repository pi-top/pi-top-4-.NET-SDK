using System.Collections.Generic;
using System.Device.Gpio;

namespace PiTop.Tests
{
  
    public class DummyGpioController : GpioController
    {
        private readonly List<int> _openPins = new List<int>();
        public ICollection<int> OpenPins => _openPins;
        private readonly Dictionary<int, List<PinValue>> _writtenData = new Dictionary<int, List<PinValue>>();
        private readonly Dictionary<int, PinMode> _pinMode = new Dictionary<int, PinMode>();

        public DummyGpioController():base(PinNumberingScheme.Logical, null)
        {
            
        }

        protected override void OpenPinCore(int pinNumber)
        {
            OpenPins.Add(pinNumber);
        }

        protected override void ClosePinCore(int pinNumber)
        {
            OpenPins.Remove(pinNumber);
        }

        public override void SetPinMode(int pinNumber, PinMode mode)
        {
            _pinMode[pinNumber] = mode;
        }

        public override PinMode GetPinMode(int pinNumber)
        {
            return _pinMode[pinNumber];
        }

        public override void Write(int pinNumber, PinValue value)
        {
            if (!_writtenData.TryGetValue(pinNumber, out var data))
            {
                data = new List<PinValue>();
                _writtenData[pinNumber] = data;
            }

            data.Add(value);
        }

    }
}