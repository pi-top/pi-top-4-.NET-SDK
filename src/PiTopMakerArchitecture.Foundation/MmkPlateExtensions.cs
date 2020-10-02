using PiTop;

namespace PiTopMakerArchitecture.Foundation
{
    public static class MmkPlateExtensions
    {
        public static MmkPlate GetOrCreateMmkPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<MmkPlate>();
        }

        public static EncodedServo GetOrCreateEncodedServo(this MmkPlate plate, EncodedServoPort port)
        {
            return plate.GetOrCreateDevice<EncodedServo>(port);
        }

        public static Motor GetOrCreateMotor(this MmkPlate plate, MotorPort port)
        {
            return plate.GetOrCreateDevice<Motor>(port);
        }
    }
}