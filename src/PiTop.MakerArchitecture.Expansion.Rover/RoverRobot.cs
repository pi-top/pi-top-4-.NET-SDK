using PiTop.Camera;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;
using Pocket;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Reactive.Linq;
using static Pocket.Logger;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class RoverRobot : IDisposable
    {
        public IPanTiltController TiltController { get; }

        public IMotionComponent MotionComponent { get; }

        public SoundSensor Sound { get; }

        public UltrasonicSensor UltrasoundBack { get; }

        public UltrasonicSensor UltrasoundFront { get; }

        public Led BackLeftLed { get; }

        public Led BackRightLed { get; }

        public Led FrontLeftLed { get; }

        public Led FrontRightLed { get; }

        public ExpansionPlate ExpansionPlate { get; }

        public IFrameSource<Image<Rgb24>> Camera { get; }

        public RoverRobot(ExpansionPlate expansionPlate, IFrameSource<Image<Rgb24>> camera, RoverRobotConfiguration configuration)
        {
            using var operation = Log.OnEnterAndExit();
            operation.Info("configuring platform ", configuration.LeftMotorPort, configuration.RightMotorPort, configuration.PanMotorPort, configuration.TiltMotorPort);

            ExpansionPlate = expansionPlate ?? throw new ArgumentNullException(nameof(expansionPlate));
            Camera = camera;

            TiltController = new PanTiltController(
                ExpansionPlate.GetOrCreateServoMotor(configuration.PanMotorPort),
                ExpansionPlate.GetOrCreateServoMotor(configuration.TiltMotorPort)
                );

            MotionComponent = new SteeringMotorController(
                ExpansionPlate.GetOrCreateEncoderMotor(configuration.LeftMotorPort),
                ExpansionPlate.GetOrCreateEncoderMotor(configuration.RightMotorPort)
                );

            FrontRightLed = ExpansionPlate.GetOrCreateLed(DigitalPort.D3, Color.Green);
            FrontLeftLed = ExpansionPlate.GetOrCreateLed(DigitalPort.D4, Color.Green);

            BackRightLed = ExpansionPlate.GetOrCreateLed(DigitalPort.D0, Color.Red);
            BackLeftLed = ExpansionPlate.GetOrCreateLed(DigitalPort.D5, Color.Red);

            UltrasoundFront = ExpansionPlate.GetOrCreateUltrasonicSensor(DigitalPort.D7);
            UltrasoundBack = ExpansionPlate.GetOrCreateUltrasonicSensor(DigitalPort.D6);

            Sound = ExpansionPlate.GetOrCreateSoundSensor(AnaloguePort.A3);

            FrontRightLed.Off();
            FrontLeftLed.Off();
            BackRightLed.Off();
            BackLeftLed.Off();
        }

        public RoverRobotState GetCurrentState()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            AllLightsOff();
            MotionComponent?.Dispose();
        }

        public void AllLightsOn()
        {
            FrontRightLed.On();
            FrontLeftLed.On();
            BackRightLed.On();
            BackLeftLed.On();
        }

        public void ToggleAllLights()
        {
            FrontRightLed.Toggle();
            FrontLeftLed.Toggle();
            BackRightLed.Toggle();
            BackLeftLed.Toggle();
        }

        public void AllLightsOff()
        {
            FrontRightLed.Off();
            FrontLeftLed.Off();
            BackRightLed.Off();
            BackLeftLed.Off();
        }

        public void BlinkAllLights(int blinkCount = 5)
        {
            Observable
                .Interval(TimeSpan.FromSeconds(0.2))
                .Take(blinkCount)
                .Subscribe(_ =>
            {
                ToggleAllLights();
            });
        }

    }
}
