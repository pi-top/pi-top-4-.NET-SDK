using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class PanTiltController : IPanTiltController
    {
        private readonly ServoMotor _panServo;
        private readonly ServoMotor _tiltServo;
        private Angle _tilt;
        private Angle _pan;

        public PanTiltController(ServoMotor panServo, ServoMotor tiltServo)
        {
            _panServo = panServo;
            _tiltServo = tiltServo;
        }

        public void Reset()
        {
            _tilt = Angle.FromDegrees(0);
            _pan = Angle.FromDegrees(0);
            SetServoToAngle(_tiltServo, Angle.FromDegrees(0));
            SetServoToAngle(_panServo, Angle.FromDegrees(0));
        }

        public void SetAngle(Angle pan, Angle tilt)
        {
            Pan = pan;
            Tilt = tilt;
        }

        public Angle Tilt
        {
            get => _tilt;
            set
            {
                if (_tilt != value)
                {
                    _tilt = value;
                    SetServoToAngle(_tiltServo, _tilt);
                }
            }
        }

        public Angle Pan
        {
            get => _pan;
            set
            {
                if (_pan != value)
                {
                    _pan = value;
                    SetServoToAngle(_panServo, _pan);
                }
            }
        }

        public void SetSpeeds(RotationalSpeed panSpeed, RotationalSpeed tiltSpeed)
        {
            _panServo.Speed = panSpeed;
            _tiltServo.Speed = tiltSpeed;
        }

        private void SetServoToAngle(ServoMotor servo, Angle angle)
        {
            servo.GoToAngle(angle);
        }
    }
}