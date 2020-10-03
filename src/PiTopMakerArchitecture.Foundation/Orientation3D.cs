using System.Numerics;

using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class Orientation3D
    {
        public Orientation3D(Angle roll, Angle pitch, Angle yaw)
        {
            Roll = roll.Unit == AngleUnit.Degree ? roll : roll.ToUnit(AngleUnit.Degree);
            Pitch = pitch.Unit == AngleUnit.Degree ? pitch : pitch.ToUnit(AngleUnit.Degree);
            Yaw = yaw.Unit == AngleUnit.Degree ? yaw : yaw.ToUnit(AngleUnit.Degree);
        }

        public Angle Roll { get; }
        public Angle Pitch { get; }
        public Angle Yaw { get; }

        public static Orientation3D FromVector(Vector3 vector, AngleUnit unit)
        {
            return new Orientation3D(
                Angle.From(vector.X, unit),
                Angle.From(vector.Y, unit),
                Angle.From(vector.Z, unit));
        }
    }
}