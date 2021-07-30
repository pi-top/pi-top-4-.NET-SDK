using System;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion
{
    /// <summary>
    /// Extensions for <see cref="UnitsNet"/>
    /// </summary>
    public static class UnitsExtensions
    {
        public static RotationalSpeed ToRotationalSpeedFromCircumference(this Speed linearSpeed,
            Length circumference)
        {

            return RotationalSpeed.FromRevolutionsPerSecond(linearSpeed.MetersPerSecond / circumference.Meters);
        }

        public static RotationalSpeed ToRotationalSpeedFromDiameter(this Speed linearSpeed,
            Length diameter)
        {

            return ToRotationalSpeedFromCircumference(linearSpeed, diameter * Math.PI);
        }

        public static Speed ToSpeedFromCircumference(this RotationalSpeed rotationalSpeed,
            Length circumference)
        {
            return Speed.FromMetersPerSecond(rotationalSpeed.RadiansPerSecond/circumference.Meters);
        }

        public static Speed ToSpeedFromDiameter(this RotationalSpeed rotationalSpeed,
            Length diameter)
        {
            return ToSpeedFromCircumference(rotationalSpeed, diameter * Math.PI);
        }
    }
}