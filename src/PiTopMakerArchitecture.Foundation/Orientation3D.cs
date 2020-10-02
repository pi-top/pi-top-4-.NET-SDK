using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class Orientation3D
    {
        public Orientation3D(Angle roll, Angle pitch, Angle yaw)
        {
            Roll = roll.ToUnit(AngleUnit.Degree);
            Pitch = pitch.ToUnit(AngleUnit.Degree);
            Yaw = yaw.ToUnit(AngleUnit.Degree);
        }

        public Angle Roll { get;  }
        public Angle Pitch { get; }
        public Angle Yaw { get;  }
    }
}