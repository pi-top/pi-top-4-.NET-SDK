using PiTop.MakerArchitecture.Foundation;
using SixLabors.ImageSharp;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class RoverRobotConfiguration
    {
        public static RoverRobotConfiguration Default => new RoverRobotConfiguration(
            EncoderMotorPort.M4,
            EncoderMotorPort.M1,
            ServoMotorPort.S1, 
            ServoMotorPort.S2,
            DigitalPort.D7,
            DigitalPort.D6,
            DigitalPort.D3,
            DigitalPort.D4,
            DigitalPort.D0,
            DigitalPort.D5, 
            AnaloguePort.A3,
            Color.Green,
            Color.Green,
            Color.Red,
            Color.Red
        );

        public RoverRobotConfiguration(
            EncoderMotorPort leftMotorPort, 
            EncoderMotorPort rightMotorPort, 
            ServoMotorPort panMotorPort, 
            ServoMotorPort tiltMotorPort, 
            DigitalPort frontUltrasoundSensorPort, 
            DigitalPort backUltrasoundSensorPort, 
            DigitalPort frontRightLedPort, 
            DigitalPort frontLeftLedPort, 
            DigitalPort backRightLedPort, 
            DigitalPort backLeftLedPort, 
            AnaloguePort soundSensorPort, 
            Color frontRightLedColor, 
            Color frontLeftLedColor, 
            Color backRightLedColor, 
            Color backLeftLedColor)
        {
            LeftMotorPort = leftMotorPort;
            RightMotorPort = rightMotorPort;
            PanMotorPort = panMotorPort;
            TiltMotorPort = tiltMotorPort;
            FrontUltrasoundSensorPort = frontUltrasoundSensorPort;
            BackUltrasoundSensorPort = backUltrasoundSensorPort;
            FrontRightLedPort = frontRightLedPort;
            FrontLeftLedPort = frontLeftLedPort;
            BackRightLedPort = backRightLedPort;
            BackLeftLedPort = backLeftLedPort;
            SoundSensorPort = soundSensorPort;
            FrontRightLedColor = frontRightLedColor;
            FrontLeftLedColor = frontLeftLedColor;
            BackRightLedColor = backRightLedColor;
            BackLeftLedColor = backLeftLedColor;
        }

        public EncoderMotorPort LeftMotorPort { get; }

        public EncoderMotorPort RightMotorPort { get; }


        public ServoMotorPort PanMotorPort { get; }

        public ServoMotorPort TiltMotorPort { get; }

        public DigitalPort FrontUltrasoundSensorPort { get; }

        public DigitalPort BackUltrasoundSensorPort { get; }

        public DigitalPort FrontRightLedPort { get; }
        
        public DigitalPort FrontLeftLedPort { get; }

        public DigitalPort BackRightLedPort { get; }

        public DigitalPort BackLeftLedPort { get; }

        public Color FrontRightLedColor { get; }

        public Color FrontLeftLedColor { get; }

        public Color BackRightLedColor { get; }

        public Color BackLeftLedColor { get; }

        public AnaloguePort SoundSensorPort { get; }
    }
}