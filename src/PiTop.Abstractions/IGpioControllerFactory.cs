namespace PiTop.Abstractions
{
    public interface IGpioControllerFactory
    {
        IGpioController GetOrCreateController();
    }
}