using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class PanTiltController : IPanTiltController
    {
        private readonly ServoMotor _panMotor;
        private readonly ServoMotor _tiltMotor;
        private Angle _tilt;
        private Angle _pan;

        public PanTiltController(ServoMotor panMotor, ServoMotor tiltMotor)
        {
            _panMotor = panMotor;
            _tiltMotor = tiltMotor;
        }

        public void Reset()
        {
            _tilt = Angle.FromDegrees(0);
            _pan = Angle.FromDegrees(0);
            SetServoToAngle(_tiltMotor, Angle.FromDegrees(0));
            SetServoToAngle(_panMotor, Angle.FromDegrees(0));
        }

        private void SetPosition(Angle pan, Angle tilt)
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
                    SetServoToAngle(_tiltMotor, _tilt);
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
                    SetServoToAngle(_panMotor, _pan);
                }
            }
        }

        private void SetServoToAngle(ServoMotor servo, Angle angle)
        {
            servo.GoToAngle(angle);
        }
    }
}