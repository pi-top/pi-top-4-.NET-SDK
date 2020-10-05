using System.Numerics;

using UnitsNet;
using UnitsNet.Units;

namespace PiTop.MakerArchitecture.Expansion
{
    public class Orientation3D
    {
        private const AngleUnit UNIT = AngleUnit.Degree;

        public Orientation3D(Angle roll, Angle pitch, Angle yaw)
        {
            Roll = ConvertIfNeeded(roll);
            Pitch = ConvertIfNeeded(pitch);
            Yaw = ConvertIfNeeded(yaw);

            static Angle ConvertIfNeeded(Angle a)
            {
                return a.Unit == UNIT ? a : a.ToUnit(UNIT);
            }
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