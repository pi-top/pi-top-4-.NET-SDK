using System;

namespace PiTop.Algorithms
{
    public class LowPassFilter : IFilter
    {
        private readonly double _alpha;
        private double _previousValue;
        public LowPassFilter(double initialValue = double.NaN, double alpha = 0.5)
        {
            _alpha = alpha;
            _previousValue = initialValue;
        }

        public double Apply(double value, DateTime? now = null)
        {
            if (double.IsNaN(_previousValue))
            {
                _previousValue = value;
                return value;
            }

            var previousValue = _previousValue;
            _previousValue = _alpha * value + (1 - _alpha) * previousValue;

            return _previousValue;
        }
    }
}