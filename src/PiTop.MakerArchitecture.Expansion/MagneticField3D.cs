using System.Numerics;

using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Expansion
{
    public class MagneticField3D
    {
        private const MagneticFieldUnit UNIT = MagneticFieldUnit.Microtesla;
        public MagneticField3D(MagneticField x, MagneticField y, MagneticField z)
        {
            X = ConvertIfNeeded(x);
            Y = ConvertIfNeeded(y);
            Z = ConvertIfNeeded(z);

            static MagneticField ConvertIfNeeded(MagneticField mf)
            {
                return mf.Unit == UNIT ? mf : mf.ToUnit(UNIT);
            }
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