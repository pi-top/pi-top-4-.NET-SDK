using System.Collections.Generic;
using PiTop.Abstractions;
using PiTop.MakerArchitecture.Foundation;
using UnitsNet;

namespace PiTop.MakerArchitecture.Expansion
{
    public interface IExpansionPlate
    {
        RotationalSpeed3D AngularVelocity { get; }
        Acceleration3D Acceleration { get; }
        Temperature Temperature { get; }
        Angle Heading { get; }
        MagneticField3D MagneticField { get; }
        Orientation3D Orientation { get; }
        IEnumerable<ServoMotor> ServoMotors { get; }
        IEnumerable<EncoderMotor> EncodedMotors { get; }
        IEnumerable<IConnectedDevice> Devices { get; }
        T GetOrCreateDevice<T>(ServoMotorPort motorPort) where T : ServoMotor;
        T GetOrCreateDevice<T>(EncoderMotorPort port) where T : EncoderMotor;
        void DisposeDevice<T>(T device) where T : IConnectedDevice;
        void Dispose();
    }
}