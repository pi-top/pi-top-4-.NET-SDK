using System;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class RoverRobot :IDisposable
    {
        public PanTiltController TiltController { get; }
        public SteeringMotorController MotorController { get; }

        public RoverRobot(ExpansionPlate expansionPlate)
        {
            ExpansionPlate = expansionPlate ?? throw new ArgumentNullException(nameof(expansionPlate));

            TiltController = new PanTiltController(
                ExpansionPlate.GetOrCreateServoMotor(ServoMotorPort.S1),
                ExpansionPlate.GetOrCreateServoMotor(ServoMotorPort.S2)
                );

            MotorController = new SteeringMotorController(
                ExpansionPlate.GetOrCreateEncoderMotor(EncoderMotorPort.M3),
                ExpansionPlate.GetOrCreateEncoderMotor(EncoderMotorPort.M1)
                );
        }

        public ExpansionPlate ExpansionPlate { get; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
