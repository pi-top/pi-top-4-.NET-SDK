using System;

using FluentAssertions;

using Microsoft.Reactive.Testing;

using UnitsNet;

using Xunit;

namespace PiTop.Algorithms.Tests
{
    public class VelocityEstimatorTests
    {
        private static Func<Acceleration> CreateSampler(params Acceleration[] accelerations)
        {
            var pos = 0;
            return () =>
            {
                var index = pos;
                pos++;
                return index < accelerations.Length ? accelerations[index] : Acceleration.Zero;
            };
        }

        [Fact]
        public void can_integrate_speed()
        {
            var scheduler = new TestScheduler();
            var accelerationSampler = CreateSampler(
                Acceleration.FromMetersPerSecondSquared(2),
                Acceleration.FromMetersPerSecondSquared(2),
                Acceleration.FromMetersPerSecondSquared(0));
            using var sensor = new VelocityEstimator(TimeSpan.FromSeconds(1), accelerationSampler, scheduler);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
            sensor.Velocity.Should().Be(Speed.FromMetersPerSecond(4));
        }

        [Fact]
        public void single_sample_calculates_zero()
        {
            var accelerationSampler = CreateSampler(
                Acceleration.FromMetersPerSecondSquared(2),
                Acceleration.FromMetersPerSecondSquared(2),
                Acceleration.FromMetersPerSecondSquared(0));
            var scheduler = new TestScheduler();
            using var sensor = new VelocityEstimator(TimeSpan.FromSeconds(1), accelerationSampler, scheduler);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(1.1).Ticks);
            sensor.Velocity.Should().Be(Speed.Zero);
        }

        [Fact]
        public void negative_accelerations_decrease_velocity()
        {

            var accelerationSampler = CreateSampler(
                Acceleration.FromMetersPerSecondSquared(2),
                Acceleration.FromMetersPerSecondSquared(-2),
                Acceleration.FromMetersPerSecondSquared(0));
            var scheduler = new TestScheduler();
            using var sensor = new VelocityEstimator(TimeSpan.FromSeconds(1), accelerationSampler, scheduler);

            scheduler.AdvanceBy(TimeSpan.FromSeconds(3).Ticks);
            sensor.Velocity.Should().Be(Speed.Zero);
        }
    }
}