using System;
using System.Linq;

namespace PiTop
{
    public static class TypeExtensions
    {
        public static string ToDisplayName(this Type source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.IsGenericType)
            {
                var args = source.GenericTypeArguments;
                var sourceString = source.FullName;
                sourceString = sourceString!.Substring(0, sourceString.IndexOf("`"));
                var argsString = string.Join(", ", args.Select(arg => ToDisplayName(arg)));
                return $"{sourceString}<{argsString}>";
            }

            return source.Name;
        }
    }
}