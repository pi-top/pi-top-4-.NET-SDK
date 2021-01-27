using PiTop.Camera;
using PiTop.MakerArchitecture.Foundation;
using PiTop.MakerArchitecture.Foundation.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;

using Pocket;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using System;
using System.Reactive.Linq;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
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
            DashBoard = new RoverDashboard(expansionPlate.PiTop4Board.Display);

            TiltController = new PanTiltController(
                ExpansionPlate.GetOrCreateServoMotor(configuration.PanMotorPort),
                ExpansionPlate.GetOrCreateServoMotor(configuration.TiltMotorPort)
                );

            MotionComponent = new SteeringMotorController(
                ExpansionPlate.GetOrCreateEncoderMotor(configuration.LeftMotorPort),
                ExpansionPlate.GetOrCreateEncoderMotor(configuration.RightMotorPort)
                );

            FrontRightLed = ExpansionPlate.GetOrCreateLed(configuration.FrontRightLedPort, configuration.FrontRightLedColor);
            FrontLeftLed = ExpansionPlate.GetOrCreateLed(configuration.FrontLeftLedPort, configuration.FrontLeftLedColor);

            BackRightLed = ExpansionPlate.GetOrCreateLed(configuration.BackRightLedPort, configuration.BackRightLedColor);
            BackLeftLed = ExpansionPlate.GetOrCreateLed(configuration.BackLeftLedPort, configuration.BackLeftLedColor);

            UltrasoundFront = ExpansionPlate.GetOrCreateUltrasonicSensor(configuration.FrontUltrasoundSensorPort);
            UltrasoundBack = ExpansionPlate.GetOrCreateUltrasonicSensor(configuration.BackUltrasoundSensorPort);

            Sound = ExpansionPlate.GetOrCreateSoundSensor(configuration.SoundSensorPort);

            FrontRightLed.Off();
            FrontLeftLed.Off();
            BackRightLed.Off();
            BackLeftLed.Off();
        }

        public RoverDashboard DashBoard { get; }

        public void WriteToDisplay(string text)
        {
            var font = SystemFonts.Collection.Find("Roboto").CreateFont(14);
           
            PiTop4Board.Instance.Display.Draw((context, cr) => {
                context.Clear(Color.Black);
                var rect = TextMeasurer.Measure(text, new RendererOptions(font));
                var x = (cr.Width - rect.Width) / 2;
                var y = (cr.Height + rect.Height) / 2;
                context.DrawText(text, font, Color.White, new PointF(0, 0));
            });
        }

        public void WriteToDisplay(Image image)
        {
            PiTop4Board.Instance.Display.Draw((context, cr) => {
                context.Clear(Color.Black);
                context.DrawImage(image,1);
            });
        }

        public RoverRobotState GetCurrentState()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            AllLightsOff();
            TiltController?.Reset();
            MotionComponent?.Stop();
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

        public void BlinkAllLights(int blinkCount = 3)
        {
            Observable
                .Interval(TimeSpan.FromSeconds(0.2))
                .Take(blinkCount * 2)
                .Subscribe(_ =>
            {
                ToggleAllLights();
            });
        }

    }
}
