using System;

using Microsoft.Psi;
using Microsoft.Psi.Components;
using PiTop.MakerArchitecture.Foundation.Sensors;
using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Foundation.Psi
{
    public static class SensorComponents
    {
        public static IProducer<bool> CreateComponent(this Button button, Pipeline pipeline)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }
            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }

            var pressedEvents = new EventSource<EventHandler<bool>, bool>(
                pipeline,
                handler => button.PressedChanged += handler,
                handler => button.PressedChanged -= handler,
                post => (sender, e) => post(e));

            return pressedEvents;
        }

        public static IProducer<Length> CreateComponent(this UltrasonicSensor ultrasonicSensor, Pipeline pipeline, TimeSpan samplingInterval)
        {
            if (ultrasonicSensor == null)
            {
                throw new ArgumentNullException(nameof(ultrasonicSensor));
            }
            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }

            return Generators.Sequence(pipeline, new Length(0,LengthUnit.Centimeter), _ => ultrasonicSensor.Distance, samplingInterval);
        }

        public static IProducer<bool> CreateComponent(this Button button, Pipeline pipeline, TimeSpan samplingInterval)
        {
            if (button == null)
            {
                throw new ArgumentNullException(nameof(button));
            }
            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }
            return Generators.Sequence(pipeline, false, _ => button.IsPressed, samplingInterval);
        }

        public static IProducer<double> CreateComponent(this LightSensor lightSensor, Pipeline pipeline, TimeSpan samplingInterval)
        {
            if (lightSensor == null)
            {
                throw new ArgumentNullException(nameof(lightSensor));
            }

            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }
            return Generators.Sequence(pipeline, 0.0, _ => lightSensor.Value, samplingInterval);
        }

        public static IProducer<double> CreateComponent(this Potentiometer potentiometer, Pipeline pipeline, TimeSpan samplingInterval)
        {
            if (potentiometer == null)
            {
                throw new ArgumentNullException(nameof(potentiometer));
            }

            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }
            return Generators.Sequence(pipeline, 0.0, _ => potentiometer.Position, samplingInterval);
        }

        public static IProducer<double> CreateComponent(this SoundSensor soundSensor, Pipeline pipeline, TimeSpan samplingInterval)
        {
            if (soundSensor == null)
            {
                throw new ArgumentNullException(nameof(soundSensor));
            }

            if (pipeline == null)
            {
                throw new ArgumentNullException(nameof(pipeline));
            }
            return Generators.Sequence(pipeline, 0.0, _ => soundSensor.Value, samplingInterval);
        }
    }
}
