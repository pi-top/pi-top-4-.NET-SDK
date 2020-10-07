using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion
{
    public static class WheelDiameters
    {
        public static Length Standard => Length.FromMeters(0.06);
        public static Length RubberTyre => Length.FromMeters(0.05);
        public static Length Tank => Length.FromMeters(0.07);
    }
}