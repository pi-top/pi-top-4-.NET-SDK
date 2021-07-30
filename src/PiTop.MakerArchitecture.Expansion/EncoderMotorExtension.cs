using System;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion
{
    /// <summary>
    /// Extensions for <see cref="EncoderMotor"/>
    /// </summary>
    public static class EncoderMotorExtension
    {
        /// <summary>
        /// Sets the motor speed from linear speed and assuming Standard diameter <see cref="WheelDiameters"/>.
        /// </summary>
        /// <param name="encoderMotor">The motor instance</param>
        /// <param name="speed"></param>
        public static void SetSpeed(this EncoderMotor encoderMotor, Speed speed)
        {
            encoderMotor.SetSpeed(speed, WheelDiameters.Standard);
        }

        /// <summary>
        /// Sets the motor speed from linear speed and wheel diameter.
        /// </summary>
        /// <param name="encoderMotor">The motor instance</param>
        /// <param name="speed">The linear speed.</param>
        /// <param name="diameter">The wheel diameter.</param>
        public static void SetSpeed(this EncoderMotor encoderMotor, Speed speed, Length diameter)
        {
            if (encoderMotor == null)
            {
                throw new ArgumentNullException(nameof(encoderMotor));
            }

            if (diameter.Meters <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(diameter), "The wheel diameter must be greater than 0");
            }
            encoderMotor.Rpm = speed.ToRotationalSpeedFromDiameter(diameter);
        }
    }
}