using System;
using System.Collections.Generic;

namespace PiTop
{
    public static class PlatePortExtensions
    {
        /// <summary>
        /// Utility to check if a port name is a value from an enum type.
        /// </summary>
        /// <typeparam name="T">The Enum type to test against.</typeparam>
        /// <returns>True if the port name is one of the Enum value.</returns>
        public static bool Is<T>(this PlatePort port) where T : Enum
        {
            var enumType = typeof(T);
            if (enumType.IsEnum)
            {
                var names = new HashSet<string>(Enum.GetNames(enumType));
                return names.Contains(port.Name);
            }

            throw new ArgumentException($"{enumType.Name} is not an enum.");
        }

    }
}