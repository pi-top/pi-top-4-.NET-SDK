using System;
using System.IO;

namespace PiTop.Camera
{
    public static class PiTopModuleExtensions
    {
        public static PiTopModule UseCamera(this PiTopModule module)
        {
            module.AddDeviceFactory<int, ICamera>(deviceType =>
            {
                var ctorSignature = new[] { typeof(int) };
                var ctor = deviceType.GetConstructor(ctorSignature);

                if (ctor != null)
                {
                    return cameraIndex =>
                        (ICamera)Activator.CreateInstance(deviceType, cameraIndex)!;

                }

                throw new InvalidOperationException(
                    $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
            });

            module.AddDeviceFactory<DirectoryInfo, FileSystemCamera>(deviceType =>
            {
                return directory => new FileSystemCamera(directory);
            });

            return module;
        }

        public static T GetOrCreateCamera<T>(this PiTopModule module, int index)
         where T : ICamera
        {
            var factory = module.GetDeviceFactory<int, ICamera>();
            AssertFactory(factory);
            return factory.GetOrCreateDevice<T>(index);
        }

        private static void AssertFactory(IConnectedDeviceFactory<int, ICamera> factory)
        {
   
            if (factory == null)
            {
                throw new NullReferenceException($"Cannot find a factory if type IConnectedDeviceFactory<int, ICamera>, make sure to configure the module calling {nameof(UseCamera)} first.");
            }
        }

        public static void DisposeDevice<T>(this PiTopModule module, T device)
            where T : ICamera
        {
            var factory = module.GetDeviceFactory<int, ICamera>();
            AssertFactory(factory);
            factory.DisposeDevice(device);
        }
    }
}
