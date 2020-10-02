using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class Acceleration3D
    {
        public Acceleration3D(Acceleration x, Acceleration y, Acceleration z)
        {
            X = x.ToUnit(AccelerationUnit.MeterPerSecondSquared);
            Y = y.ToUnit(AccelerationUnit.MeterPerSecondSquared);
            Z = z.ToUnit(AccelerationUnit.MeterPerSecondSquared);
        }

        public Acceleration X { get;  }
        public Acceleration Y { get; }
        public Acceleration Z { get; }
    }
}