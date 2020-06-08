using System;
using System.Collections.Generic;
using System.Linq;

namespace PiTop
{
    public class DeviceFactory<TPort, TDevice> : IDeviceFactory<TPort, TDevice>, IDisposable
        where TDevice : IPiTopConnectedDevice
    {
        private readonly Dictionary<TPort, TDevice> _devices = new Dictionary<TPort, TDevice>();
        private readonly Func<Type, Func<TPort, TDevice>> _defaultDeviceFactoryGenerator;
        private readonly Dictionary<Type, Func<TPort, TDevice>> _factories = new Dictionary<Type, Func<TPort, TDevice>>();
        public DeviceFactory(Func<Type, Func<TPort, TDevice>> defaultDeviceFactoryGenerator = null)
        {

            defaultDeviceFactoryGenerator ??= deviceType =>
            {
                var ctorSignature = new[] { typeof(TPort) };
                var ctor = deviceType.GetConstructor(ctorSignature);
                if (ctor != null)
                {
                    return devicePort => (TDevice)Activator.CreateInstance(deviceType, devicePort);

                }

                throw new InvalidOperationException(
                    $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
            };

            _defaultDeviceFactoryGenerator =
                deviceType =>
                {
                    if (!typeof(TDevice).IsAssignableFrom(deviceType))
                    {
                        throw new InvalidOperationException(
                            $"Device of type ${deviceType} do not derive from {typeof(TDevice)}");
                    }

                    return defaultDeviceFactoryGenerator(deviceType);

                };
        }

        public T GetOrCreateDevice<T>(TPort port) where T : TDevice
        {
            if (!_factories.TryGetValue(typeof(T), out var factory))
            {
                factory = _defaultDeviceFactoryGenerator(typeof(T));
                _factories[typeof(T)] = factory;
            }

            return GetOrCreateDevice<T>(port, factory);
        }

        public T GetOrCreateDevice<T>(TPort port, Func<TPort, TDevice> deviceFactory) where T : TDevice
        {
            if (deviceFactory == null)
            {
                throw new ArgumentNullException(nameof(deviceFactory));
            }

            if (!_devices.TryGetValue(port, out var device))
            {
                device = deviceFactory(port);
                _devices[port] = device;
                device.Initialize();
            }

            return (T)device;

        }

        public IEnumerable<TDevice> Devices => _devices.Select(e => e.Value);

        public void Dispose()
        {
            var devices = _devices.Select(e => e.Value).ToList();

            foreach (var disposable in devices)
            {
                disposable.Dispose();
            }
        }
    }
}