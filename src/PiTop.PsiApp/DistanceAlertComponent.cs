using System;

using Microsoft.Psi;

using PiTopMakerArchitecture.Foundation.Components;

namespace PiTop.PsiApp
{
    public class DistanceAlertComponent
    {
        private readonly Led[] _alertDisplay;
        private double _currentThreshold;
        private double _currentDistance;

        public DistanceAlertComponent(Pipeline pipeline, Led[] alertDisplay)
        {
            _alertDisplay = alertDisplay;
            Threshold = pipeline.CreateReceiver<double>(this, OnThreshold, nameof(Threshold));
            Distance = pipeline.CreateReceiver<double>(this, OnDistance, nameof(Distance));

        }

        private void OnDistance(Message<double> distanceMessage)
        {
            _currentDistance = Math.Round(distanceMessage.Data, 1);

            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            var alertLevel = ComputeAlertLevel();
            if (Math.Abs(alertLevel) < 0.001)
            {
                _alertDisplay[0].On();
                for (var i = 1; i < _alertDisplay.Length; i++)
                {
                    _alertDisplay[i].Off();
                }
            }
            else
            {

                var limit = Math.Floor(_alertDisplay.Length * alertLevel);
                for (var i = 0; i < _alertDisplay.Length; i++)
                {
                    if (i <= limit)
                    {
                        _alertDisplay[i].On();
                    }
                    else
                    {
                        _alertDisplay[i].Off();
                    }
                }
            }
        }

        private double ComputeAlertLevel()
        {
            if (_currentDistance > _currentThreshold)
            {
                return 0;
            }

            var level = Math.Min(Math.Max((_currentThreshold - _currentDistance) / _currentThreshold, 0), 1);
            return level;

        }

        public Receiver<double> Distance { get; set; }

        private void OnThreshold(Message<double> thresholdMessage)
        {
            var thresholdValue = Math.Round(thresholdMessage.Data, 3);
            if (Math.Abs(thresholdValue - _currentThreshold) > 0.01)
            {
                _currentThreshold = thresholdValue;

                UpdateDisplay();

            }
        }

        public Receiver<double> Threshold { get; }
    }
}