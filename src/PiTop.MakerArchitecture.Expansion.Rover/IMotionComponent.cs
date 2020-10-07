using System.Numerics;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public interface IMotionComponent
    {
        void SetSpeed(Speed speed, Vector2 direction);
        public void Stop();
    }
}