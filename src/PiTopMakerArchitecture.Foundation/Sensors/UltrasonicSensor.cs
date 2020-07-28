using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

using PiTop;
using PiTop.Abstractions;
using UnitsNet;

namespace PiTopMakerArchitecture.Foundation.Sensors
{
    public class UltrasonicSensor : DigitalPortDeviceBase
    {
        private const int MaxDistance = 300;
        private readonly int _echoPin;
        private readonly int _triggerPin;
        private readonly Stopwatch _timer = new Stopwatch();

        private int _lastMeasurement = 0;


        public UltrasonicSensor(DigitalPort port, IGpioControllerFactory controllerFactory) : base(port, controllerFactory)
        {
            (_echoPin, _triggerPin) = port.ToPinPair();
        }

        public Length Distance => GetDistance();

        protected override void OnConnection()
        {
            base.OnConnection();
            AddToDisposables(Controller.OpenPin(_echoPin, PinMode.Input));
            AddToDisposables(Controller.OpenPin(_triggerPin, PinMode.Output));
            Controller.Write(_triggerPin, PinValue.Low);
            Controller.Read(_echoPin);
        }

        private Length GetDistance()
        {
            
            for (var i = 0; i < 10; i++)
            {
                if (TryGetDistance(out var result))
                {
                    return Length.FromCentimeters(result);
                }
            }

            throw new InvalidOperationException("Could not get reading from the sensor");
        }

        private bool TryGetDistance(out double result)
        {
            // Time when we give up on looping and declare that reading failed
            // 100ms was chosen because max measurement time for this sensor is around 24ms for 400cm
            // additionally we need to account 60ms max delay.
            // Rounding this up to a 100 in case of a context switch.
            long hangTicks = Environment.TickCount + 100;
            _timer.Reset();

            // Measurements should be 60ms apart, in order to prevent trigger signal mixing with echo signal
            // ref https://components101.com/sites/default/files/component_datasheet/HCSR04%20Datasheet.pdf
            while (Environment.TickCount - _lastMeasurement < 60)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(Environment.TickCount - _lastMeasurement));
            }

            // Trigger input for 10uS to start ranging
            Controller.Write(_triggerPin, PinValue.High);
            Thread.Sleep(TimeSpan.FromMilliseconds(0.01));
            Controller.Write(_triggerPin, PinValue.Low);

            // Wait until the echo pin is HIGH (that marks the beginning of the pulse length we want to measure)
            while (Controller.Read(_echoPin) == PinValue.Low)
            {
                if (Environment.TickCount - hangTicks > 0)
                {
                    result = default;
                    return false;
                }
            }

            _lastMeasurement = Environment.TickCount;

            _timer.Start();

            // Wait until the pin is LOW again, (that marks the end of the pulse we are measuring)
            while (Controller.Read(_echoPin) == PinValue.High)
            {
                if (Environment.TickCount - hangTicks > 0)
                {
                    result = default;
                    return false;
                }
            }

            _timer.Stop();

            var elapsed = _timer.Elapsed;

            // distance = (time / 2) × velocity of sound (34300 cm/s)
            result = elapsed.TotalMilliseconds / 2.0 * 34.3;
            if (result > MaxDistance)
            {
                // result is more than sensor supports
                // something went wrong
                result = default;
                return false;
            }

            return true;
        }
    }
}