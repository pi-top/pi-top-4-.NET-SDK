using PiTop;

namespace PiTopMakerArchitecture.Foundation
{
    public static class MmkPlateExtensions
    {
        public static MmkPlate GetOrCreateMmkPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<MmkPlate>();
        }

        public static ServoMotor GetOrCreateServoMotor(this MmkPlate plate, ServoMotorPort motorPort)
        {
            return plate.GetOrCreateDevice<ServoMotor>(motorPort);
        }

        public static EncoderMotor GetOrCreateEncoderMotor(this MmkPlate plate, EncoderMotorPort port)
        {
            return plate.GetOrCreateDevice<EncoderMotor>(port);
        }
    }
}