using System;

namespace Prototype
{
    public abstract class JoystickEvent
    {
        private readonly uint _timeStamp;

        public DateTime TimeStamp => new DateTime(1970, 1, 1).AddSeconds(_timeStamp);

        protected JoystickEvent(uint timeStamp)
        {
            _timeStamp = timeStamp;
        }
    }

}