using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class MagneticField3D
    {
        public MagneticField3D(MagneticField x, MagneticField y, MagneticField z)
        {
            X = x.ToUnit(MagneticFieldUnit.Tesla);
            Y = y.ToUnit(MagneticFieldUnit.Tesla);
            Z = z.ToUnit(MagneticFieldUnit.Tesla);
        }

        public MagneticField X { get; }
        public MagneticField Y { get; }
        public MagneticField Z { get; }
    }
}