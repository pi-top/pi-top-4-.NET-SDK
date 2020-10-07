using System.Numerics;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class SteeringMotorController : IMotionComponent
    {
        private readonly EncoderMotor _leftEncoderMotor;
        private readonly EncoderMotor _rightEncoderMotor;

        public SteeringMotorController(EncoderMotor leftEncoderMotor, EncoderMotor rightEncoderMotor)
        {
            _leftEncoderMotor = leftEncoderMotor;
            _rightEncoderMotor = rightEncoderMotor;

            _leftEncoderMotor.ForwardDirection = ForwardDirection.Clockwise;
            _rightEncoderMotor.ForwardDirection = ForwardDirection.CounterClockwise;
        }

        public void SetSpeed(Speed speed, Vector2 direction)
        {
            var n = Vector2.Normalize(direction);
            _leftEncoderMotor.Speed = speed * n.X;
            _rightEncoderMotor.Speed = speed * n.X;
        }

        public void Stop()
        {
            _leftEncoderMotor.Stop();
            _rightEncoderMotor.Stop();
        }
    }
}