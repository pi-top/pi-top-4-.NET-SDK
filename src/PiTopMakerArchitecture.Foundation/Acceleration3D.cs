using System.Numerics;

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

        public Acceleration X { get; }
        public Acceleration Y { get; }
        public Acceleration Z { get; }

        public static Acceleration3D FromVector(Vector3 vector, AccelerationUnit unit)
        {
            return new Acceleration3D(
                UnitsNet.Acceleration.From(vector.X, unit),
                UnitsNet.Acceleration.From(vector.Y, unit),
                UnitsNet.Acceleration.From(vector.Z, unit));
        }
    }
}