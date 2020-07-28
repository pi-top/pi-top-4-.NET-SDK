using System;

namespace PiTop
{
    public class PiTopButton
    {
        private PiTopButtonState _state = PiTopButtonState.Released;
        public event EventHandler<bool>? PressedChanged;
        public event EventHandler<EventArgs>? Pressed;
        public event EventHandler<EventArgs>? Released;

        public PiTopButtonState State
        {
            get => _state;
            internal set
            {
                if (_state != value)
                {
                    PressedChanged?.Invoke(this, _state == PiTopButtonState.Pressed);

                    switch (_state)
                    {
                        case PiTopButtonState.Pressed:
                            Pressed?.Invoke(this, EventArgs.Empty);
                            break;
                        case PiTopButtonState.Released:
                            Released?.Invoke(this, EventArgs.Empty);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                _state = value;
            }
        }
    }
}