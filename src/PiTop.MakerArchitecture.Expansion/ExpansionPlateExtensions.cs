namespace PiTop.MakerArchitecture.Expansion
{
    public static class ExpansionPlateExtensions
    {
        public static ExpansionPlate GetOrCreateExpansionPlate(this PiTop4Board module)
        {
            return module.GetOrCreatePlate<ExpansionPlate>();
        }

        public static ServoMotor GetOrCreateServoMotor(this IExpansionPlate plate, ServoMotorPort motorPort)
        {
            return plate.GetOrCreateDevice<ServoMotor>(motorPort);
        }

        public static EncoderMotor GetOrCreateEncoderMotor(this IExpansionPlate plate, EncoderMotorPort port)
        {
            return plate.GetOrCreateDevice<EncoderMotor>(port);
        }
    }
}