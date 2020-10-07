using System;

using PiTop.Abstractions;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion
{
    public class ServoMotor : IConnectedDevice
    {
        private readonly SMBusDevice _controller;
        private Angle _zeroPoint;
        private RotationalSpeed _defaultSpeed;
        public ServoMotorPort Port { get; }

        private const int ANGLE_RANGE = 180;
        private const int SPEED_RANGE = 100;
        private const int MIN_PULSE_WIDTH_MICRO_S = 500;
        private const int MAX_PULSE_WIDTH_MICRO_S = 2500;



        private const byte REGISTER_MIN_PULSE_WIDTH = 0x4A;
        private const byte REGISTER_MAX_PULSE_WIDTH = 0x4B;
        private const byte REGISTER_PWM_FREQUENCY = 0x4C;
        private const int PWM_FREQUENCY = 60;
        private const double PWM_PERIOD = 1.0 / PWM_FREQUENCY;
        private const int DUTY_REGISTER_RANGE = 4095;


        private byte RegisterControlMode=> (byte) (0x50 + Port);
        private byte RegisterSpeed => (byte)(0x56 + Port);
        private byte RegisterAngleAndSpeed => (byte)(0x5C + Port);

        public ServoMotor(ServoMotorPort port, SMBusDevice controller)
        {
            _controller = controller;
            Port = port;
            ZeroPoint = Angle.Zero;
            _defaultSpeed = RotationalSpeed.FromDegreesPerSecond(50);
        }

        private Angle ZeroPoint
        {
            get => _zeroPoint;
            set
            {
                if (value.Degrees < -90 || value.Degrees > 90)
                {
                    throw new ArgumentOutOfRangeException(nameof(ZeroPoint), "ZeroPoint must be an angle between -90 and 90 degrees");
                }
                _zeroPoint = value;
                
            }
        }

        public void GoToAngle(Angle angle)
        {
            GoToAngle(angle, _defaultSpeed);
        }

        public void GoToAngle(Angle angle, RotationalSpeed speed)
        {
            if (Math.Abs(angle.Degrees) > ANGLE_RANGE)
            {
                throw new ArgumentOutOfRangeException(nameof(angle), $"Angle value must be in range [-{ANGLE_RANGE},{ANGLE_RANGE}] degrees.");
            }

            if (Math.Abs(speed.DegreesPerSecond) > SPEED_RANGE)
            {
                throw new ArgumentOutOfRangeException(nameof(speed), $"Angle value must be in range [-{SPEED_RANGE},{SPEED_RANGE}] degrees/second.");
            }

            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Stop();
        }

        private void Stop()
        {
            throw new NotImplementedException();
        }

        public void Connect()
        {
            _controller.WriteWord(REGISTER_MIN_PULSE_WIDTH, MIN_PULSE_WIDTH_MICRO_S);
            _controller.WriteWord(REGISTER_MAX_PULSE_WIDTH, MAX_PULSE_WIDTH_MICRO_S);
           _controller.WriteByte(REGISTER_PWM_FREQUENCY, PWM_FREQUENCY);

        }
    }
}