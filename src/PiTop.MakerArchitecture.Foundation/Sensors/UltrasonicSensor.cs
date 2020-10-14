using PiTop.Abstractions;

using Pocket;

using System;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

using UnitsNet;

using static Pocket.Logger;

namespace PiTop.MakerArchitecture.Foundation.Sensors
{
    public class UltrasonicSensor : DigitalPortDeviceBase
    {
        private const int MAX_DISTANCE = 300;
        private readonly int _echoPin;
        private readonly int _triggerPin;
        private readonly Stopwatch _timer = new Stopwatch();

        private int _lastMeasurement = 0;
        private readonly ManualResetEvent _echoReceived;

        public UltrasonicSensor(DigitalPort port, IGpioControllerFactory controllerFactory) : base(port, controllerFactory)
        {
            (_echoPin, _triggerPin) = port.ToPinPair();
            _echoReceived = new ManualResetEvent(false);
            AddToDisposables(_echoReceived);
        }

        public Length Distance => GetDistance();

        protected override void OnConnection()
        {
            base.OnConnection();
            AddToDisposables(Controller.OpenPinAsDisposable(_echoPin, PinMode.Input));
            AddToDisposables(Controller.OpenPinAsDisposable(_triggerPin, PinMode.Output));
            Controller.Write(_triggerPin, PinValue.Low);
            Controller.Read(_echoPin);
            AddToDisposables(Controller.RegisterCallbackForPinValueChangedEventAsDisposable(_echoPin, PinEventTypes.Falling | PinEventTypes.Rising, (_, s) =>
            {
                switch (s.ChangeType)
                {
                    case PinEventTypes.Rising:
                        _timer.Start();
                        break;
                    case PinEventTypes.Falling:
                        _timer.Stop();
                        _echoReceived.Set();
                        break;
                }
            }));
        }

        private Length GetDistance()
        {

            for (var i = 0; i < 10; i++)
            {
                // Measurements should be 60ms apart, in order to prevent trigger signal mixing with echo signal
                // ref https://components101.com/sites/default/files/component_datasheet/HCSR04%20Datasheet.pdf
                var waitMillis = _lastMeasurement + 60 - Environment.TickCount;
                while (waitMillis > 0)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(waitMillis));
                    waitMillis = _lastMeasurement + 60 - Environment.TickCount;
                }

                if (TryGetDistance(out var result))
                {
                    _lastMeasurement = Environment.TickCount; // ensure that we wait 60ms
                    return Length.FromCentimeters(result);
                }

                _lastMeasurement = Environment.TickCount; // ensure that we wait 60ms
            }

            throw new SensorReadException($"Could not get reading from the sensor on port {Port}");
        }

        private bool TryGetDistance(out double result)
        {
            using var operation = Log.OnEnterAndConfirmOnExit();

            operation.Info("Trigger starting");
            _timer.Reset();
            _echoReceived.Reset();
            // Trigger input for 10uS to start ranging
            Controller.Write(_triggerPin, PinValue.High);
            Thread.Sleep(TimeSpan.FromMilliseconds(0.01));
            Controller.Write(_triggerPin, PinValue.Low);
            operation.Info("Trigger sent");

            if (!_echoReceived.WaitOne(TimeSpan.FromMilliseconds(100)))
            {
                operation.Error("Timeout waiting for reading, Timeout value 100 ms");
                result = default;
                return false;
            }

            var elapsed = _timer.Elapsed;
            operation.Info($"elapsed {elapsed.TotalMilliseconds:F3} ms");

            // distance = (time / 2) × velocity of sound (34300 cm/s)
            result = elapsed.TotalMilliseconds / 2.0 * 34.3;
            if (result > MAX_DISTANCE)
            {
                // result is more than sensor supports
                // something went wrong
                operation.Error($"Out of range reading : {result} is above threshold {MAX_DISTANCE}");
                result = default;
                return false;
            }

            operation.Info($"distance {result:F1} cm");

            operation.Succeed();
            return true;
        }
    }
}