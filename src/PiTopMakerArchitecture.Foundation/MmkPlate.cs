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

        public Angle Heading => GetHeading();

        public MagneticField3D MagneticField => GetMagneticField();

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
            var acceleration = GetAcceleration();
            var x = acceleration.X.StandardGravity;
            var y = acceleration.Y.StandardGravity;
            var z = acceleration.Z.StandardGravity;

            var xSquare = x * x;
            var ySquare = y * y;
            var zSquare = z * z;
            return new Orientation3D(
                 Angle.FromRadians(Math.Atan(x / Math.Sqrt(ySquare + zSquare))),
            Angle.FromRadians(Math.Atan(y / Math.Sqrt(xSquare + zSquare))),
            Angle.FromRadians(Math.Atan(z / Math.Sqrt(xSquare + ySquare)))
                );
        }

        private Mpu9250 GetOrCreateMPU9250()
        {
            return _imu ??= CreateAndCalibrate();

            Mpu9250 CreateAndCalibrate()
            {
                var device = new Mpu9250(GetOrCreateMcu());
                device.CalibrateMagnetometer();
                device.CalibrateGyroscopeAccelerometer();
                return device;
            }
        }

        private Angle GetHeading()
        {
            var magneticField = GetMagneticField();
            return Angle.FromRadians(Math.Atan2(magneticField.Y.Microteslas, magneticField.X.Microteslas));
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
            return Acceleration3D.FromVector(reading, AccelerationUnit.StandardGravity);
        }

        private MagneticField3D GetMagneticField()
        {
            var imu = GetOrCreateMPU9250();
            var reading = imu.ReadMagnetometer();
            return MagneticField3D.FromVector(reading, MagneticFieldUnit.Microtesla);
        }

        private RotationalSpeed3D GetAngularVelocity()
        {
            var imu = GetOrCreateMPU9250();
            var reading = imu.GetGyroscopeReading();
            return RotationalSpeed3D.FromVector(reading, RotationalSpeedUnit.DegreePerSecond);

        }
    }
}