using System.Numerics;

using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Expansion
{
    public class Acceleration3D
    {
        private const AccelerationUnit UNIT = AccelerationUnit.MeterPerSecondSquared;

        public Acceleration3D(Acceleration x, Acceleration y, Acceleration z)
        {
            X = ConvertIfNeeded(x);
            Y = ConvertIfNeeded(y);
            Z = ConvertIfNeeded(z);

            static Acceleration ConvertIfNeeded(Acceleration a)
            {
                return a.Unit == UNIT ? a : a.ToUnit(UNIT);
            }
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