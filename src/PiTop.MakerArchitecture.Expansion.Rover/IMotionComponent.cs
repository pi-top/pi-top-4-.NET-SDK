using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public interface IMotionComponent
    {
        void SetSpeedAndSteering(Speed speed, RotationalSpeed headingChange);
        public void Stop();
    }
}