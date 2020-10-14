using System;

namespace PiTop.Algorithms
{
    public interface IFilter
    {
        double Apply(double value, DateTime? now = null);
    }
}