using System;
using System.Collections.Generic;

using lobe;

using SixLabors.ImageSharp;

using UnitsNet;

namespace PiTop.Interactive.Rover.ML
{
    public class CameraSector : IEquatable<CameraSector>
    {
        public bool Equals(CameraSector other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Pan.Equals(other.Pan) && Tilt.Equals(other.Tilt);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CameraSector) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Pan, Tilt);
        }

        public static bool operator ==(CameraSector left, CameraSector right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CameraSector left, CameraSector right)
        {
            return !Equals(left, right);
        }

        private sealed class PanTiltEqualityComparer : IEqualityComparer<CameraSector>
        {
            public bool Equals(CameraSector x, CameraSector y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Pan.Equals(y.Pan) && x.Tilt.Equals(y.Tilt);
            }

            public int GetHashCode(CameraSector obj)
            {
                return HashCode.Combine(obj.Pan, obj.Tilt);
            }
        }

        public static IEqualityComparer<CameraSector> PanTiltComparer { get; } = new PanTiltEqualityComparer();

        public Angle Pan { get; }
        public Angle Tilt { get; }

        public CameraSector(Angle pan, Angle tilt)
        {
            Pan = pan;
            Tilt = tilt;
        }

        public bool Marked { get; set; }

        public Image CapturedFrame { get; set; }

        public ClassificationResults ClassificationResults { get; set; }

        public void Reset()
        {
            CapturedFrame = null;
            ClassificationResults = null;
            Marked = false;
        }

        public static IEnumerable<CameraSector> CreateSectors(int columnCount, int rowCount, Angle minPan, Angle maxPan,
            Angle minTilt, Angle maxTilt)
        {
            var sectors = new List<CameraSector>(columnCount * rowCount);
            var pan = minPan;
            var panStep = (maxPan - minPan) / (columnCount - 1);

            var tilt = minTilt;
            var tiltStep = (maxTilt - minTilt) / (rowCount - 1);
            for (var row = 0; row < rowCount; row++)
            {
                for (var column = 0; column < columnCount; column++)
                {
                    sectors.Add(new CameraSector(
                        pan + (panStep * column),
                        tilt + (tiltStep * row)
                        ));
                }
            }

            return sectors;
        }
    }
}