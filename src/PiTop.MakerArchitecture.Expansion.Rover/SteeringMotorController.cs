using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class SteeringMotorController : IMotionComponent, IDisposable
    {
        private readonly EncoderMotor _leftMotor;
        private readonly EncoderMotor _rightMotor;

        private Length _wheelbase = Length.FromCentimeters(16);
        private Speed _speed;
        private RotationalSpeed _turnRate;
        private Subject<(Speed, Speed)> _writeSpeed;
        private IDisposable _disposable;

        public SteeringMotorController(EncoderMotor leftMotor, EncoderMotor rightMotor)
        {
            _leftMotor = leftMotor;
            _rightMotor = rightMotor;

            // assuming motors have same max speed
            MaxSteering = RotationalSpeed.FromRadiansPerSecond((_leftMotor.MaxSpeed.MetersPerSecond * 2) / _wheelbase.Meters);

            _leftMotor.ForwardDirection = ForwardDirection.Clockwise;
            _rightMotor.ForwardDirection = ForwardDirection.CounterClockwise;

            _writeSpeed = new Subject<(Speed left, Speed right)>();
            _disposable = _writeSpeed.Sample(TimeSpan.FromMilliseconds(100)).Subscribe(
                speeds =>
                {
                    _leftMotor.Speed = speeds.Item1;
                    Console.WriteLine($"  _leftMotor.Speed = {(speeds.Item1).CentimetersPerSecond}");
                    _rightMotor.Speed = speeds.Item2;
                    Console.WriteLine($" _rightMotor.Speed = {(speeds.Item2).CentimetersPerSecond}");

                });
        }

        public Speed Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                SetMotorSpeeds();
            }
        }
        public RotationalSpeed Steering
        {
            get => _turnRate;
            set
            {
                _turnRate = value;
                SetMotorSpeeds();
            }
        }

        public RotationalSpeed MaxSteering { get; }

        /// <summary>
        /// Set overall forward speed and turnrate
        /// </summary>
        /// <param name="speed">how fast are we going forward</param>
        /// <param name="turnRate">how fast are we turning right</param>
        public void SetSpeedAndSteering(Speed speed, RotationalSpeed turnRate)
        {
            Console.WriteLine($"[speed={speed.CentimetersPerSecond}, turnRate={turnRate.DegreesPerSecond}]");
            _speed = speed;
            _turnRate = turnRate;
            SetMotorSpeeds();
        }

        private void SetMotorSpeeds()
        {
            // speed = (l+r)/2 [m/s]
            // turnRate = (l-r)/wheelbase [rad/s]
            // ==>
            // r = speed - 1/2*turnRate*wheelbase
            // l = speed + 1/2*turnRate*wheelbase

            var diff = Speed.FromMetersPerSecond(_turnRate.RadiansPerSecond / 2 * _wheelbase.Meters);

            _writeSpeed.OnNext((_speed + diff, _speed - diff));
        }

        public void Stop()
        {
            _leftMotor.Stop();
            _rightMotor.Stop();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            Stop();
        }
    }
}