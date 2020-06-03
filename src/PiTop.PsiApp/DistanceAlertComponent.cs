using System;
using Microsoft.Psi;
using PiTopMakerArchitecture.Foundation.Components;

namespace PiTop.PsiApp
{
    public class DistanceAlertComponent
    {
        private readonly Led[] _alertDisplay;
        private double _currentThreshold;

        public DistanceAlertComponent(Pipeline pipeline, Led[] alertDisplay)
        {
            _alertDisplay = alertDisplay;
            Threshold = pipeline.CreateReceiver<double>(this, OnThreshold, nameof(Threshold));
            Distance = pipeline.CreateReceiver<double>(this, OnDistance, nameof(Distance));
        }

        private void OnDistance(Message<double> distanceMessage)
        {
            var distance = distanceMessage.Data;

            if (distance < _currentThreshold)
            {
                _alertDisplay[0].On();
                for (int i = 1; i < _alertDisplay.Length; i++)
                {
                    _alertDisplay[i].Off();
                }
            }
            else
            {
                var ratio = Math.Round((distance - _currentThreshold) / _currentThreshold, 2);
                var limit = Math.Ceiling(_alertDisplay.Length * ratio);
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

        public Receiver<double> Distance { get; set; }

        private void OnThreshold(Message<double> thresholdMessage)
        {
            _currentThreshold = thresholdMessage.Data;
        }

        public Receiver<double> Threshold { get; }
    }
}