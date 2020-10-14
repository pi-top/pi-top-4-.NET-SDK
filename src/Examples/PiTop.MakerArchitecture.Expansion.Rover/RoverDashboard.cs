using System;
using PiTop.Abstractions;
using SixLabors.ImageSharp;

namespace PiTop.MakerArchitecture.Expansion.Rover
{
    public class RoverDashboard
    {
        private readonly Display _display;

        public RoverDashboard(Display display)
        {
            _display = display ?? throw new ArgumentNullException(nameof(display));
        }

        public void Clear()
        {
            _display.Clear();
        }

        public Image GetSnapshot()
        {
            return _display.Capture();
        }
    }
}