namespace PiTop.MakerArchitecture.Expansion
{
    public static class MathHelpers
    {
        public static double Interpolate(this double point, double domainMin, double domainMax, double codomainMin, double codomainMax)
        {
            var clamped = point > domainMax ? domainMax : point < domainMin ? domainMin : point;

            var pos = (clamped - domainMin) / (domainMax - domainMin);

            var interp = (pos * (codomainMax - codomainMin)) + codomainMin;
            return interp;

        }

        public static double Interpolate(this short point, double codomainMin, double codomainMax)
        {
            return Interpolate(point, short.MinValue, short.MaxValue, codomainMin, codomainMax);
        }

    }
}
