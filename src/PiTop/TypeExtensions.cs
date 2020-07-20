using System;
using System.Linq;

namespace PiTop
{
    public static class TypeExtensions
    {
        public static string ToDisplayName(this Type source, bool fullName = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.IsGenericType)
            {
                var args = source.GenericTypeArguments;
                var sourceString = fullName ? source.FullName : source.Name;
                sourceString = sourceString!.Substring(0, sourceString.IndexOf("`"));
                var argsString = string.Join(", ", args.Select(arg => ToDisplayName(arg)));
                return $"{sourceString}<{argsString}>";
            }

            return (fullName ? source.FullName : source.Name) ?? string.Empty;
        }
    }
}