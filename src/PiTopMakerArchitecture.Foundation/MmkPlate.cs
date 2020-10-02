using System;

using PiTop;
using PiTop.Abstractions;

using UnitsNet;

namespace PiTopMakerArchitecture.Foundation
{
    public class MmkPlate : FoundationPlate
    {
        private readonly ConnectedDeviceFactory<EncodedServoPort, EncodedServo> _encodedServoFactory;
        private readonly ConnectedDeviceFactory<MotorPort, Motor> _motorFactory;

        public RotationalSpeed3D AngularVelocity => GetAngularVelocity();

        public Acceleration3D Acceleration => GetAcceleration();

        public Temperature Temperature => GetTemperature();

        public Angle Heading => GetAngle();

        public Orientation3D Orientation => GetOrientation();



        public MmkPlate(PiTop4Board module) : base(module)
        {
            _encodedServoFactory = new ConnectedDeviceFactory<EncodedServoPort, EncodedServo>(deviceType =>
            {
                var ctorSignature = new[] { typeof(EncodedServoPort), typeof(IGpioControllerFactory) };
                throw new InvalidOperationException(
                    $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
            });

            _motorFactory = new ConnectedDeviceFactory<MotorPort, Motor>(
                deviceType =>
                {

                    var ctorSignature = new[] { typeof(MotorPort), typeof(IGpioControllerFactory) };
                    throw new InvalidOperationException(
                        $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
                });

            RegisterForDisposal(_encodedServoFactory);
            RegisterForDisposal(_motorFactory);
        }

        public T GetOrCreateDevice<T>(EncodedServoPort port) where T : EncodedServo
        {
            return _encodedServoFactory.GetOrCreateDevice<T>(port);
        }

        public T GetOrCreateDevice<T>(MotorPort port) where T : Motor
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