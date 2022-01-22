namespace MaSch.Core.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="StringBuilder"/> class.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Returns the index of the start of the contents in a StringBuilder.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> in which to search.</param>
        /// <param name="value">The string to find.</param>
        /// <param name="startIndex">The starting index.</param>
        /// <param name="ignoreCase">if set to <c>true</c> it will ignore case.</param>
        /// <returns>The index of the first appearance of <paramref name="value"/> inside the <see cref="StringBuilder"/> starting from <paramref name="startIndex"/>.</returns>
        /// <see href="https://stackoverflow.com/questions/1359948/why-doesnt-stringbuilder-have-indexof-method#6601226"/>
        public static int IndexOf(this StringBuilder sb, string value, int startIndex, bool ignoreCase)
        {
            int index;
            int length = value.Length;
            int maxSearchLength = (sb.Length - length) + 1;

            if (ignoreCase)
            {
                for (int i = startIndex; i < maxSearchLength; ++i)
                {
                    if (char.ToLower(sb[i]) == char.ToLower(value[0]))
                    {
                        index = 1;
                        while ((index < length) && (char.ToLower(sb[i + index]) == char.ToLower(value[index])))
                            ++index;

                        if (index == length)
                            return i;
                    }
                }

                return -1;
            }

            for (int i = startIndex; i < maxSearchLength; ++i)
            {
                if (sb[i] == value[0])
                {
                    index = 1;
                    while ((index < length) && (sb[i + index] == value[index]))
                        ++index;

                    if (index == length)
                        return i;
                }
            }

            return -1;
        }
    }
}
