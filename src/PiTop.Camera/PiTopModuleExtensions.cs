using System;

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

            return module;
        }

        public static T GetOrCreateCamera<T>(this PiTopModule module, int index)
         where T : ICamera
        {
            var factory = module.GetDeviceFactory<int, ICamera>();
            return factory.GetOrCreateDevice<T>(index);
        }
    }
}
