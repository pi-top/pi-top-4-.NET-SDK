namespace Prototype
{
    public class ButtonEvent : JoystickEvent
    {
        public Button Button { get; }
        public bool Pressed { get; }

        internal ButtonEvent(uint timeStamp, Button button, bool pressed) : base(timeStamp)
        {
            Button = button;
            Pressed = pressed;
        }

    }

}