using MaSch.Core;
using MaSch.Presentation.Wpf.Themes;

namespace MaSch.Presentation.Wpf.Models;

/// <summary>
/// Represents a reference to a theme value.
/// </summary>
/// <seealso cref="ICloneable" />
public class ThemeValueReference : ICloneable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeValueReference"/> class.
    /// </summary>
    /// <param name="key">The key that the reference points to..</param>
    public ThemeValueReference(string key)
        : this(key, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ThemeValueReference"/> class.
    /// </summary>
    /// <param name="key">The key that the reference points to..</param>
    /// <param name="property">The property that the reference points to..</param>
    public ThemeValueReference(string key, string? property)
    {
        _ = Guard.NotNullOrEmpty(key, nameof(key));

        CustomKey = key;
        Property = property;
    }

    /// <summary>
    /// Gets or sets the key that the reference points to.
    /// </summary>
    public ThemeKey Key
    {
        get => (ThemeKey)Enum.Parse(typeof(ThemeKey), CustomKey);
        set => CustomKey = value.ToString();
    }

    /// <summary>
    /// Gets or sets the custom key that the reference points to.
    /// </summary>
    public string CustomKey { get; set; }

    /// <summary>
    /// Gets or sets the property that the reference points to.
    /// </summary>
    public string? Property { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        return obj is ThemeValueReference other && other.CustomKey == CustomKey && other.Property == Property;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return (CustomKey, Property).GetHashCode();
    }

    /// <inheritdoc/>
    public object Clone()
    {
        return new ThemeValueReference(CustomKey, Property);
    }
}
