using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public interface IPanTiltController
    {
        void Reset();
        Angle Tilt { get; set; }
        Angle Pan { get; set; }
    }
}