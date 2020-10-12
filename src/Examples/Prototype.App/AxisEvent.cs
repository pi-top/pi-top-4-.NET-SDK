namespace Prototype
{
    public class AxisEvent : JoystickEvent
    {
        public Axis Axis { get; }
        public short Position { get; }

        internal AxisEvent(uint timeStamp, Axis axis, short position) : base(timeStamp)
        {
            Axis = axis;
            Position = position;
        }

    }

}