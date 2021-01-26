using System.Globalization;

namespace MaSch.Common.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="CultureInfo"/>.
    /// </summary>
    public static class CultureInfoExtensions
    {
        /// <summary>
        /// Determines the neutral culture of the <see cref="CultureInfo"/>.
        /// </summary>
        /// <param name="culture">The non-neutral <see cref="CultureInfo"/>.</param>
        /// <returns>Returns the neutral <see cref="CultureInfo"/> of this <see cref="CultureInfo"/>.</returns>
        public static CultureInfo? GetNeutralCulture(this CultureInfo culture)
        {
            Guard.NotNull(culture, nameof(culture));

            var current = culture;
            while (current.IsNeutralCulture == false && !Equals(current, CultureInfo.InvariantCulture)) 
                current = current.Parent;
            return current;
        }
    }
}
