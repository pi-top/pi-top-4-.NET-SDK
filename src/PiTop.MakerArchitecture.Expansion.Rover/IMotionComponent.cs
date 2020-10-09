using System;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public interface IMotionComponent : IDisposable
    {
        void SetSpeedAndSteering(Speed speed, RotationalSpeed headingChange);
        public void Stop();
    }
}