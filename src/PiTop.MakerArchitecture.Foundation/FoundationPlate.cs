namespace PiTop.MakerArchitecture.Foundation
{
    public class FoundationPlate : PiTopPlate
    {
       

        public FoundationPlate(PiTop4Board module) : base(module)
        {
            RegisterPorts<DigitalPort>();
            RegisterPorts<AnaloguePort>();

        }
    }
}