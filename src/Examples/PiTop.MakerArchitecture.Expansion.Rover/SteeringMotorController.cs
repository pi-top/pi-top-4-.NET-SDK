using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Pocket;
using static Pocket.Logger;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class SteeringMotorController : IMotionComponent
    {
        private readonly EncoderMotor _leftMotor;
        private readonly EncoderMotor _rightMotor;

        private Length _wheelbase = Length.FromCentimeters(16);
        private Speed _speed;
        private RotationalSpeed _turnRate;
        private readonly Subject<(RotationalSpeed, RotationalSpeed)> _writeSpeed;
        private readonly IDisposable _disposable;

        public Speed MaxSpeed => Speed.FromMetersPerSecond(WheelCircumference.Meters * EncoderMotor.MaxRpm.RevolutionsPerSecond);

        public SteeringMotorController(EncoderMotor leftMotor, EncoderMotor rightMotor) : this(leftMotor, rightMotor, WheelDiameters.Standard)
        {

        }

        public SteeringMotorController(EncoderMotor leftMotor, EncoderMotor rightMotor, Length wheelDiameter)
        {
            _leftMotor = leftMotor;
            _rightMotor = rightMotor;
            WheelCircumference = Length.FromMeters(wheelDiameter.Meters * Math.PI);


            // assuming motors have same max speed
            MaxSteering = RotationalSpeed.FromRadiansPerSecond(MaxSpeed.MetersPerSecond * 2 / _wheelbase.Meters);

            _leftMotor.ForwardDirection = ForwardDirection.Clockwise;
            _rightMotor.ForwardDirection = ForwardDirection.CounterClockwise;

            _writeSpeed = new Subject<(RotationalSpeed left, RotationalSpeed right)>();

            _disposable = _writeSpeed
                .Sample(TimeSpan.FromMilliseconds(100))
                .Merge(_writeSpeed.Where(s => s.Item1 == RotationalSpeed.Zero || s.Item2 == RotationalSpeed.Zero))
                .DistinctUntilChanged()
                .Subscribe(
                speeds =>
                {
                    using var operation = Log.OnExit("SpeedStream");

                    _leftMotor.Rpm = speeds.Item1;
                    _rightMotor.Rpm = speeds.Item2;

                    operation.Info("Motors = L[{_leftMotor.Rpm}] R[{_rightMotor.Rpm}]", _leftMotor.Rpm, _rightMotor.Rpm);

                });
        }

        public Length WheelCircumference { get; }

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
        /// Set overall forward speed and turnRate
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

            _writeSpeed.OnNext((
                (_speed + diff).ToRotationalSpeedFromCircumference(WheelCircumference), 
                (_speed - diff).ToRotationalSpeedFromCircumference(WheelCircumference)
                ));
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

        public void SetPower(double rightMotorPower, double leftMotorPower)
        {
            _leftMotor.Power = leftMotorPower;
            _rightMotor.Power = rightMotorPower;
        }
    }
}