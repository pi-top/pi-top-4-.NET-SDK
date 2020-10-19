using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using UnitsNet;

namespace PiTop.Algorithms
{
    public class VelocitySensor : IDisposable
    {
        private readonly IDisposable _sampler;
        private (DateTimeOffset Timestamp, Acceleration Accelecation)? _previousSample;
        private IScheduler _scheduler;

        public VelocitySensor(TimeSpan samplingFrequency, Func<Acceleration> accelerationSampler, IScheduler samplingScheduler = null)
        {
            Velocity = Speed.Zero;
            _scheduler = samplingScheduler ?? TaskPoolScheduler.Default;
            var samplingFrequency1 = samplingFrequency;
            if (samplingFrequency1.TotalMilliseconds <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(samplingFrequency), "Sampling frequency must be greater than 0 ms");
            }
            var accelerationSampler1 = accelerationSampler ?? throw new ArgumentNullException(nameof(accelerationSampler));
            _sampler = Observable
                .Interval(samplingFrequency1, _scheduler)
                .Select(_ => (_scheduler.Now, accelerationSampler1()))
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


        public Speed Velocity { get; private set; }



        public void Dispose()
        {
            _sampler.Dispose();
        }
    }
}