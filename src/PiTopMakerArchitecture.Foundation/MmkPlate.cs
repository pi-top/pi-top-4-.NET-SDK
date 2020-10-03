using System;
using System.Device.I2c;
using Iot.Device.Imu;

using PiTop;

using UnitsNet;
using UnitsNet.Units;

namespace PiTopMakerArchitecture.Foundation
{
    public class MmkPlate : FoundationPlate
    {
        private readonly ConnectedDeviceFactory<ServoMotorPort, ServoMotor> _encodedServoFactory;
        private readonly ConnectedDeviceFactory<EncoderMotorPort, EncoderMotor> _motorFactory;
        private I2cDevice? _mcu;
        private Mpu9250? _imu;

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
                        (ServoMotor)Activator.CreateInstance(deviceType, devicePort, GetOrCreateMcu())!;

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
                            (EncoderMotor)Activator.CreateInstance(deviceType, devicePort, GetOrCreateMcu())!;

                    }
                    throw new InvalidOperationException(
                        $"Cannot find suitable constructor for type {deviceType}, looking for signature {ctorSignature}");
                });

            RegisterForDisposal(_encodedServoFactory);
            RegisterForDisposal(_motorFactory);
            RegisterForDisposal(() =>
            {
                _imu?.Dispose();
                _mcu?.Dispose();
            });
        }

        protected I2cDevice GetOrCreateMcu()
        {
            return _mcu ??= PiTop4Board.GetOrCreateI2CDevice(I2C_ADDRESS_PLATE_MCU);
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
            var imu = GetOrCreateMPU9250();
            var reading = imu.GetGyroscopeReading();
            return new Orientation3D(
                Angle.From(reading.X, AngleUnit.Degree),
            Angle.From(reading.Y, AngleUnit.Degree),
            Angle.From(reading.Z, AngleUnit.Degree));
        }

        private Mpu9250 GetOrCreateMPU9250()
        {
            return _imu ??= CreateAndCalibrate(GetOrCreateMcu());
            

            Mpu9250 CreateAndCalibrate(I2cDevice i2cBus)
            {
                var device = new Mpu9250(i2cBus);
                device.CalibrateMagnetometer();
                device.CalibrateGyroscopeAccelerometer();
                return device;
            }
        }

        private Angle GetAngle()
        {
            throw new NotImplementedException();
        }

        private Temperature GetTemperature()
        {
            var imu = GetOrCreateMPU9250();
            return imu.GetTemperature();
        }

        private Acceleration3D GetAcceleration()
        {
            var imu = GetOrCreateMPU9250();
            var reading = imu.GetAccelerometer();
            return new Acceleration3D(
                UnitsNet.Acceleration.From(reading.X, AccelerationUnit.StandardGravity),
                UnitsNet.Acceleration.From(reading.Y, AccelerationUnit.StandardGravity),
                UnitsNet.Acceleration.From(reading.Z, AccelerationUnit.StandardGravity));
        }

        private RotationalSpeed3D GetAngularVelocity()
        {
            throw new NotImplementedException();
        }
    }
}