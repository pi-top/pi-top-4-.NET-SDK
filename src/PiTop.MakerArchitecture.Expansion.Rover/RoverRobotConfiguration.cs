namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class RoverRobotConfiguration
    {
        public static RoverRobotConfiguration Defaulf => new RoverRobotConfiguration(EncoderMotorPort.M4,
            EncoderMotorPort.M1, ServoMotorPort.S1, ServoMotorPort.S2
        );
        public RoverRobotConfiguration(EncoderMotorPort leftMotorPort, EncoderMotorPort rightMotorPort, ServoMotorPort panMotorPort, ServoMotorPort tiltMotorPort)
        {
            LeftMotorPort = leftMotorPort;
            RightMotorPort = rightMotorPort;
            PanMotorPort = panMotorPort;
            TiltMotorPort = tiltMotorPort;
        }

        public EncoderMotorPort LeftMotorPort { get; }

        public EncoderMotorPort RightMotorPort { get; }



        public ServoMotorPort PanMotorPort { get; }

        public ServoMotorPort TiltMotorPort { get; }
    }
}