using System.Numerics;

using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class RotationalSpeed3D
    {
        public RotationalSpeed3D(RotationalSpeed x, RotationalSpeed y, RotationalSpeed z)
        {
            X = x.Unit == RotationalSpeedUnit.DegreePerSecond ? x : x.ToUnit(RotationalSpeedUnit.DegreePerSecond);
            Y = y.Unit == RotationalSpeedUnit.DegreePerSecond ? y : y.ToUnit(RotationalSpeedUnit.DegreePerSecond);
            Z = z.Unit == RotationalSpeedUnit.DegreePerSecond ? z : z.ToUnit(RotationalSpeedUnit.DegreePerSecond);
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