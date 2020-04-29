using System;

namespace PiTopMakerArchitecture.Foundation
{
    public abstract class CssColor : DisplayPropertyBase
    {
        public string Value { get; protected set; }
    }

    public class NamedCssColor : CssColor
    {
        public NamedCssColor(string colorName)
        {
            if (string.IsNullOrWhiteSpace(colorName))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(colorName));
            }
            Value = colorName;
        }
    }

    public class RgbaCssColor : CssColor
    {
        public RgbaCssColor(double r, double g, double b, double a)
        {
            Value = $"rgba({r}, {g}, {b}, {a})";
        }
    }
}