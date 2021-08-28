using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    /// <inheritdoc/>
    public abstract class CliCommandMemberInfo : ICliCommandMemberInfo
    {
        private readonly ObjectExtensionDataStorage _extensionStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CliCommandMemberInfo"/> class.
        /// </summary>
        /// <param name="extensionStorage">The extension data storage.</param>
        /// <param name="command">The command this member belongs to.</param>
        /// <param name="property">The property this member represents.</param>
        protected CliCommandMemberInfo(ObjectExtensionDataStorage extensionStorage, ICliCommandInfo command, PropertyInfo property)
        {
            _extensionStorage = Guard.NotNull(extensionStorage, nameof(extensionStorage));
            Command = Guard.NotNull(command, nameof(command));
            Property = Guard.NotNull(property, nameof(property));

            if (Property.GetIndexParameters()?.Any() == true)
                throw new ArgumentException($"The property cannot be an indexer.", nameof(property));
            if (Property.GetAccessors(true).Any(x => x.IsStatic))
                throw new ArgumentException($"The property \"{property.Name}\" cannot be static.", nameof(property));
            if (!Property.CanRead || !Property.CanWrite)
                throw new ArgumentException($"The property \"{property.Name}\" needs to have a setter and getter.", nameof(property));
        }

        /// <inheritdoc/>
        public ICliCommandInfo Command { get; }

        /// <inheritdoc/>
        public string PropertyName => Property.Name;

        /// <inheritdoc/>
        public Type PropertyType => Property.PropertyType;

        /// <inheritdoc/>
        public abstract bool IsRequired { get; }

        /// <inheritdoc/>
        public abstract object? DefaultValue { get; }

        /// <inheritdoc/>
        public abstract string? HelpText { get; }

        /// <inheritdoc/>
        public abstract bool Hidden { get; }

        /// <summary>
        /// Gets the property this member represents.
        /// </summary>
        protected PropertyInfo Property { get; }

        /// <inheritdoc/>
        public virtual object? GetValue(object options)
        {
            Guard.NotNull(options, nameof(options));
            return Property.GetValue(options);
        }

        /// <inheritdoc/>
        public virtual bool HasValue(object options)
        {
            Guard.NotNull(options, nameof(options));
            return _extensionStorage[options].TryGetValue(GetHasValueKey(), out object? objHasValue) && objHasValue is bool hasValue && hasValue;
        }

        /// <inheritdoc/>
        public virtual void SetDefaultValue(object options)
        {
            Guard.NotNull(options, nameof(options));
            if (typeof(IEnumerable).IsAssignableFrom(PropertyType) && PropertyType != typeof(string))
                SetValue(options, Array.Empty<object?>(), true);
            else
                SetValue(options, DefaultValue ?? PropertyType.GetDefault(), true);
        }

        /// <inheritdoc/>
        public virtual void SetValue(object options, object? value)
        {
            Guard.NotNull(options, nameof(options));
            SetValue(options, value, false);
        }

        private void SetValue(object options, object? value, bool isDefault)
        {
            if (typeof(IEnumerable).IsAssignableFrom(Property.PropertyType) && Property.PropertyType != typeof(string) && !isDefault)
            {
                var currentValue = Property.GetValue(options);
                if (currentValue is IEnumerable e1 && value is IEnumerable e2)
                    value = e1.ToGeneric().Concat(e2.ToGeneric());
            }

            _extensionStorage[options][GetHasValueKey()] = !isDefault;

            try
            {
                Property.SetValue(options, value.ConvertTo(Property.PropertyType, CultureInfo.InvariantCulture));
            }
            catch (InvalidCastException ex)
            {
                throw new FormatException($"Could not parse value to {Property.PropertyType.FullName}.", ex);
            }
        }

        private string GetHasValueKey() => $"HasValue_{PropertyName}";
    }
}
