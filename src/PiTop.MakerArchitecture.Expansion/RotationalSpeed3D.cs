using System.Numerics;

using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Expansion
{
    public class RotationalSpeed3D
    {
        private const RotationalSpeedUnit UNIT = RotationalSpeedUnit.DegreePerSecond;

        public RotationalSpeed3D(RotationalSpeed x, RotationalSpeed y, RotationalSpeed z)
        {
            X = ConvertIfNeeded(x);
            Y = ConvertIfNeeded(y);
            Z = ConvertIfNeeded(z);

            static RotationalSpeed ConvertIfNeeded(RotationalSpeed rs)
            {
                return rs.Unit == UNIT ? rs : rs.ToUnit(UNIT);
            }
        }

        public RotationalSpeed X { get; }
        public RotationalSpeed Y { get; }
        public RotationalSpeed Z { get; }

        public static RotationalSpeed3D FromVector(Vector3 vector, RotationalSpeedUnit unit)
        {
            return new RotationalSpeed3D(
                RotationalSpeed.From(vector.X, unit),
                RotationalSpeed.From(vector.Y, unit),
                RotationalSpeed.From(vector.Z, unit));
        }
    }
}