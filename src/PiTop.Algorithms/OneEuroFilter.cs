using System;

namespace PiTop.Algorithms
{
    /// <summary>
    /// Smoothing filter adapted from https://github.com/jaantollander/OneEuroFilter
    /// </summary>
    public class OneEuroFilter
    {
        private DateTime _previousTime;
        private double _previousValue;
        private double _previousDelta;
        private readonly double _minCutoff;
        private readonly double _beta;
        private readonly double _cutoff;

        public OneEuroFilter(DateTime? currentTime = null, double initialValue = double.NaN, double delta = 0.0, double minCutoff = 1.0, double beta = 0.0,
            double cutoff = 1.0)
        {
            _previousTime = currentTime ?? DateTime.UtcNow;
            _previousValue = initialValue;
            _previousDelta = delta;
            _minCutoff = minCutoff;
            _beta = beta;
            _cutoff = cutoff;
        }

        public double Apply(double value, DateTime? now = null)
        {
            now = now?.ToUniversalTime() ?? DateTime.UtcNow;

            if (double.IsNaN(_previousValue))
            {
                _previousValue = value;
                _previousTime = now.Value;
                return value;
            }

            var elapsedMilliseconds = (now.Value - _previousTime).TotalMilliseconds;

            // The filtered derivative of the signal.
            var alpha = SmoothingFactor(elapsedMilliseconds, _cutoff);
            var dx = (value - _previousValue) / elapsedMilliseconds;
            var dxHat = ExponentialSmoothing(alpha, dx, _previousDelta);

            // The filtered signal.
            var cutoff = _minCutoff + _beta * Math.Abs(dxHat);
            alpha = SmoothingFactor(elapsedMilliseconds, cutoff);
            var xHat = ExponentialSmoothing(alpha, value, _previousValue);

            // Memorize the previous values.
            _previousValue = xHat;
            _previousDelta = dxHat;
            _previousTime = now.Value;

            return xHat;
        }

        private static double SmoothingFactor(double elapsedMilliseconds, double cutoff)
        {
            var r = 2 * Math.PI * cutoff * elapsedMilliseconds;
            return r / (r + 1);
        }


        private static double ExponentialSmoothing(double alpha, double value, double previousValue)
        {
            return alpha * value + (1 - alpha) * previousValue;
        }
    }
}
