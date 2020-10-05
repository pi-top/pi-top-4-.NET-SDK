using System;
using System.Collections.Generic;

namespace PiTop.MakerArchitecture.Foundation
{
    public static class PortExtensions
    {
        public static DigitalPort NextDigitalPort(this DigitalPort digitalPort, bool wrap = false)
        {
            switch (digitalPort)
            {
                case DigitalPort.D0:
                    return DigitalPort.D1;
                case DigitalPort.D1:
                    return DigitalPort.D2;
                case DigitalPort.D2:
                    return DigitalPort.D3;
                case DigitalPort.D3:
                    return DigitalPort.D4;
                case DigitalPort.D4:
                    return DigitalPort.D5;
                case DigitalPort.D5:
                    return DigitalPort.D6;
                case DigitalPort.D6:
                    return DigitalPort.D7;
                case DigitalPort.D7:
                    if (!wrap)
                    {
                        throw new InvalidOperationException("D7 is the last DigitalPort.");
                    }
                    return DigitalPort.D1;
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitalPort), digitalPort, null);
            }
        }

        public static IEnumerable<DigitalPort> GetDigitalPortRange(this DigitalPort first, int count)
        {
            var current = first;
            yield return current;
            for (int i = 0; i < (count - 1); i++)
            {
                current = current.NextDigitalPort();
                yield return current;
            }
        }

        public static (int pin1, int pin2) ToPinPair(this DigitalPort digitalPort)
        {
            switch (digitalPort)
            {
                case DigitalPort.D0:
                    return (22, 23);
                case DigitalPort.D1:
                    return (24, 25);
                case DigitalPort.D2:
                    return (26, 27);
                case DigitalPort.D3:
                    return (5, 6);
                case DigitalPort.D4:
                    return (07, 08);
                case DigitalPort.D5:
                    return (10, 11);
                case DigitalPort.D6:
                    return (12, 13);
                case DigitalPort.D7:
                    return (15, 16);
                default:
                    throw new ArgumentOutOfRangeException(nameof(digitalPort), digitalPort, null);
            }
        }

        public static (int pin1, int pin2) ToPinPair(this AnaloguePort analoguePort)
        {
            switch (analoguePort)
            {
                case AnaloguePort.A0:
                    return (0, 1);
                case AnaloguePort.A1:
                    return (2, 3);
                case AnaloguePort.A2:
                    return (4, 5);
                case AnaloguePort.A3:
                    return (6, 7);
                default:
                    throw new ArgumentOutOfRangeException(nameof(analoguePort), analoguePort, null);
            }
        }
    }
}