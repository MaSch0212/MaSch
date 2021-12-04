using MaSch.Core;
using System.ComponentModel;
using System.Windows.Markup;

namespace MaSch.Presentation.Wpf.Markup;

/// <summary>
/// A <see cref="MarkupExtension"/> that returns all elements in an enumeration.
/// </summary>
/// <seealso cref="MarkupExtension" />
public class EnumerationExtension : MarkupExtension
{
    private Type _enumType;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnumerationExtension"/> class.
    /// </summary>
    /// <param name="enumType">Type of the enum.</param>
    public EnumerationExtension(Type enumType)
    {
        _ = Guard.NotNull(enumType, nameof(enumType));

        _enumType = ValidateType(enumType);
    }

    /// <summary>
    /// Gets the type of the enum.
    /// </summary>
    /// <exception cref="ArgumentException">Type must be an Enum.</exception>
    public Type EnumType
    {
        get => _enumType;
        private set
        {
            if (_enumType == value)
                return;
            _enumType = ValidateType(value);
        }
    }

    /// <summary>
    /// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
    /// </summary>
    /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
    /// <returns>
    /// The object value to set on the property where the extension is applied.
    /// </returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var enumValues = Enum.GetValues(EnumType);

        return (
          from object enumValue in enumValues
          select new EnumerationMember
          {
              Value = enumValue,
              Description = GetDescription(enumValue),
          }).ToArray();
    }

    private static Type ValidateType(Type value)
    {
        var enumType = Nullable.GetUnderlyingType(value) ?? value;

        if (!enumType.IsEnum)
            throw new ArgumentException("Type must be an Enum.");

        return enumType;
    }

    private string? GetDescription(object enumValue)
    {
        var enumValueName = enumValue.ToString();
        if (enumValueName == null)
            return enumValueName;
        return EnumType
            .GetField(enumValueName)?
            .GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() is DescriptionAttribute descriptionAttribute
          ? descriptionAttribute.Description
          : enumValueName;
    }

    /// <summary>
    /// Represents a member of an enumeration.
    /// </summary>
    public class EnumerationMember
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object? Value { get; set; }
    }
}
