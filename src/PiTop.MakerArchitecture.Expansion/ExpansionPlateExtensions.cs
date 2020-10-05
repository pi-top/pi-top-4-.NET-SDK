namespace PiTop.MakerArchitecture.Expansion
{
    public static class ExpansionPlateExtensions
    {
        public static ExpansionPlate GetOrCreateMmkPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<ExpansionPlate>();
        }

        public static ServoMotor GetOrCreateServoMotor(this ExpansionPlate plate, ServoMotorPort motorPort)
        {
            return plate.GetOrCreateDevice<ServoMotor>(motorPort);
        }

        public static EncoderMotor GetOrCreateEncoderMotor(this ExpansionPlate plate, EncoderMotorPort port)
        {
            return plate.GetOrCreateDevice<EncoderMotor>(port);
        }
    }
}