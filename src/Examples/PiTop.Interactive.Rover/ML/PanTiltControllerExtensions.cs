using PiTop.MakerArchitecture.Expansion.Rover;

namespace PiTop.Interactive.Rover.ML
{
    public static class PanTiltControllerExtensions
    {
        public static void GoToSector(this IPanTiltController controller, CameraSector sector)
        {
            controller.Pan = sector.Pan;
            controller.Tilt = sector.Tilt;
        }
    }
}