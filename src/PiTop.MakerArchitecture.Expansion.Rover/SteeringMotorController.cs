using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class SteeringMotorController
    {
        private readonly EncoderMotor _leftEncoderMotor;
        private readonly EncoderMotor _rightEncoderMotor;


        public SteeringMotorController(EncoderMotor leftEncoderMotor, EncoderMotor rightEncoderMotor)
        {
            _leftEncoderMotor = leftEncoderMotor;
            _rightEncoderMotor = rightEncoderMotor;
          
        }

        
    }
}