using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class RotationalSpeed3D
    {
        public RotationalSpeed3D(RotationalSpeed x, RotationalSpeed y, RotationalSpeed z)
        {
            X = x.ToUnit(RotationalSpeedUnit.DegreePerSecond);
            Y = y.ToUnit(RotationalSpeedUnit.DegreePerSecond);
            Z = z.ToUnit(RotationalSpeedUnit.DegreePerSecond);
        }

        public RotationalSpeed X { get; }
        public RotationalSpeed Y { get; }
        public RotationalSpeed Z { get;  }
    }
}