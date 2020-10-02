using System;
using System.Device.I2c;

using PiTop;

using UnitsNet;

namespace PiTopMakerArchitecture.Foundation
{
    public class MmkPlate : FoundationPlate
    {
        private readonly ConnectedDeviceFactory<ServoMotorPort, ServoMotor> _encodedServoFactory;
        private readonly ConnectedDeviceFactory<EncoderMotorPort, EncoderMotor> _motorFactory;

        public RotationalSpeed3D AngularVelocity => GetAngularVelocity();

        public Acceleration3D Acceleration => GetAcceleration();

        public Temperature Temperature => GetTemperature();

        public Angle Heading => GetAngle();

        public Orientation3D Orientation => GetOrientation();

        private const int I2C_ADDRESS_PLATE_MCU = 0x04;

        public MmkPlate(PiTop4Board module) : base(module)
        {
            _encodedServoFactory = new ConnectedDeviceFactory<ServoMotorPort, ServoMotor>(deviceType =>
            {
                var ctorSignature = new[] { typeof(ServoMotorPort), typeof(I2cDevice) };
                var ctor = deviceType.GetConstructor(ctorSignature);
                if (ctor != null)
                {
                    return devicePort =>
                        (ServoMotor)Activator.CreateInstance(deviceType, devicePort, GetOrCreateMCU())!;

                }
                throw new InvalidOperationException(
                    $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
            });

            _motorFactory = new ConnectedDeviceFactory<EncoderMotorPort, EncoderMotor>(
                deviceType =>
                {

                    var ctorSignature = new[] { typeof(EncoderMotorPort), typeof(I2cDevice) };
                    var ctor = deviceType.GetConstructor(ctorSignature);
                    if (ctor != null)
                    {
                        return devicePort =>
                            (EncoderMotor)Activator.CreateInstance(deviceType, devicePort, GetOrCreateMCU())!;

                    }
                    throw new InvalidOperationException(
                        $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
                });

            RegisterForDisposal(_encodedServoFactory);
            RegisterForDisposal(_motorFactory);
        }

        private I2cDevice GetOrCreateMCU()
        {
            throw new NotImplementedException();
        }

        public T GetOrCreateDevice<T>(ServoMotorPort motorPort) where T : ServoMotor
        {
            return _encodedServoFactory.GetOrCreateDevice<T>(motorPort);
        }

        public T GetOrCreateDevice<T>(EncoderMotorPort port) where T : EncoderMotor
        {
            return _motorFactory.GetOrCreateDevice<T>(port);
        }

        private Orientation3D GetOrientation()
        {
            throw new NotImplementedException();
        }

        private Angle GetAngle()
        {
            throw new NotImplementedException();
        }

        private Temperature GetTemperature()
        {
            throw new NotImplementedException();
        }

        private Acceleration3D GetAcceleration()
        {
            throw new NotImplementedException();
        }

        private RotationalSpeed3D GetAngularVelocity()
        {
            throw new NotImplementedException();
        }
    }
}