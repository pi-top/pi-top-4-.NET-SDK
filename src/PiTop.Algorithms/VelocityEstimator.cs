using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

using UnitsNet;

namespace PiTop.Algorithms
{
    /// <summary>
    /// Calculates the velocity by sampling acceleration values.
    /// The initial velocity of the sensor is 0 m/s and it will require a pair of sample for first value .
    /// </summary>
    public class VelocityEstimator : IDisposable
    {
        private readonly IDisposable _sampler;
        private (DateTimeOffset Timestamp, Acceleration Accelecation)? _previousSample;

        public VelocityEstimator(TimeSpan samplingFrequency, Func<Acceleration> accelerationSampler, IScheduler samplingScheduler = null)
        {
            Velocity = Speed.Zero;
            var scheduler = samplingScheduler ?? TaskPoolScheduler.Default;
            
            if (samplingFrequency.TotalMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(samplingFrequency), "Sampling frequency must be greater than 0 ms");
            }

            accelerationSampler = accelerationSampler ?? throw new ArgumentNullException(nameof(accelerationSampler));
            _sampler = Observable
                .Interval(samplingFrequency, scheduler)
                .Select(_ =>
                {
                    var acceleration = accelerationSampler();
                    return (scheduler.Now, acceleration);
                })
                .Subscribe(OnNext);
        }

        private void OnNext((DateTimeOffset Timestamp, Acceleration Accelecation) sample)
        {
            var newSample = sample;

            if (_previousSample != null)
            {
                var dt = newSample.Timestamp - _previousSample.Value.Timestamp;
                var a = _previousSample.Value.Accelecation;
                Velocity = Speed.FromMetersPerSecond(Velocity.MetersPerSecond + dt.TotalSeconds * a.MetersPerSecondSquared);
            }

            _previousSample = newSample;
        }

        /// <summary>
        /// The velocity
        /// </summary>
        public Speed Velocity { get; private set; }



        public void Dispose()
        {
            _sampler.Dispose();
        }
    }
}