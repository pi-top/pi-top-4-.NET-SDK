using System.Numerics;
using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class MagneticField3D
    {
        public MagneticField3D(MagneticField x, MagneticField y, MagneticField z)
        {
            X = x.ToUnit(MagneticFieldUnit.Microtesla);
            Y = y.ToUnit(MagneticFieldUnit.Microtesla);
            Z = z.ToUnit(MagneticFieldUnit.Microtesla);
        }

        public MagneticField X { get; }
        public MagneticField Y { get; }
        public MagneticField Z { get; }

        public static MagneticField3D FromVector(Vector3 vector, MagneticFieldUnit unit)
        {
            return new MagneticField3D(
                MagneticField.From(vector.X, unit),
                MagneticField.From(vector.Y, unit),
                MagneticField.From(vector.Z, unit));
        }
    }
}