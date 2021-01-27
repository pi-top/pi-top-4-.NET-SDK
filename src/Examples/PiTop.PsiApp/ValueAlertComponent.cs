using System;

using Microsoft.Psi;

using PiTop.MakerArchitecture.Foundation.Components;

using Pocket;

using static Pocket.Logger;

namespace PiTop.PsiApp
{
    public class ValueAlertComponent
    {
        private readonly Led[] _alertDisplay;
        private double _currentThreshold;
        private double _currentValue;
        private double _currentAlertLevel;

        public ValueAlertComponent(Pipeline pipeline, Led[] alertDisplay)
        {
            _alertDisplay = alertDisplay;
            Threshold = pipeline.CreateReceiver<double>(this, OnThreshold, nameof(Threshold));
            Value = pipeline.CreateReceiver<double>(this, OnValue, nameof(Value));
        }

        private void OnValue(Message<double> valueMessage)
        {
            _currentValue = Math.Round(valueMessage.Data, 1);
            UpdateAlertLevel();
        }

        private void UpdateAlertLevel()
        {
            var alertLevel = ComputeAlertLevel();
            if (Math.Abs(_currentAlertLevel - alertLevel) > 0.05)
            {
                using var op = Log.OnEnterAndExit();
                op.Info($"Current Level: {_currentAlertLevel}");
                _currentAlertLevel = alertLevel;
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
        }

        private double ComputeAlertLevel()
        {
            if (_currentValue > _currentThreshold)
            {
                return 0;
            }

            var level = Math.Round(Math.Min(Math.Max((_currentThreshold - _currentValue) / _currentThreshold, 0), 1), 2);
            return level;
        }

        public Receiver<double> Value { get; set; }

        private void OnThreshold(Message<double> thresholdMessage)
        {
            var thresholdValue = Math.Round(thresholdMessage.Data, 3);
            if (Math.Abs(thresholdValue - _currentThreshold) > 0.01)
            {
                _currentThreshold = thresholdValue;
                UpdateAlertLevel();

            }
        }

        public Receiver<double> Threshold { get; }
    }
}