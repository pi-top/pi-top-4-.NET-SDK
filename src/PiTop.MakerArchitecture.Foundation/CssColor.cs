using System;

namespace PiTop.MakerArchitecture.Foundation
{
    public abstract class CssColor : DisplayPropertyBase
    {
        public string? Value { get; protected set; }
    }

    public class NamedCssColor : CssColor
    {
        public NamedCssColor(string colorName)
        {
            if (colorName == null) throw new ArgumentNullException(nameof(colorName));
            if (string.IsNullOrWhiteSpace(colorName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(colorName));
            }
            Value = colorName;
        }
    }

    public class RgbaCssColor : CssColor
    {
        public RgbaCssColor(byte r, byte g, byte b, double a)
        {
            Value = $"rgba({r}, {g}, {b}, {a:0.0})";
        }
    }
}