using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    public abstract class CliCommandMemberInfo : ICliCommandMemberInfo
    {
        private readonly Dictionary<object, bool> _optionsToHasValue = new();

        protected PropertyInfo Property { get; }

        public ICliCommandInfo Command { get; }
        public string PropertyName => Property.Name;
        public Type PropertyType => Property.PropertyType;

        public abstract bool IsRequired { get; }
        public abstract object? DefaultValue { get; }
        public abstract string? HelpText { get; }

        protected CliCommandMemberInfo(ICliCommandInfo command, PropertyInfo property)
        {
            Command = Guard.NotNull(command, nameof(command));
            Property = Guard.NotNull(property, nameof(property));
        }

        public virtual object? GetValue(object options)
            => Property.GetValue(options);

        public virtual bool HasValue(object options)
            => _optionsToHasValue.TryGetValue(options, out var hasValue) && hasValue;

        public virtual void SetDefaultValue(object options)
        {
            if (typeof(IEnumerable).IsAssignableFrom(PropertyType) && PropertyType != typeof(string))
                SetValue(options, Array.Empty<object?>(), true);
            else
                SetValue(options, DefaultValue ?? PropertyType.GetDefault(), true);
        }

        public virtual void SetValue(object options, object? value)
            => SetValue(options, value, false);

        private void SetValue(object options, object? value, bool isDefault)
        {
            if (typeof(IEnumerable).IsAssignableFrom(Property.PropertyType) && Property.PropertyType != typeof(string))
            {
                var currentValue = Property.GetValue(options);
                if (currentValue is IEnumerable e1 && value is IEnumerable e2)
                    value = e1.ToGeneric().Concat(e2.ToGeneric());
            }

            Property.SetValue(options, value.ConvertTo(Property.PropertyType));

            if (_optionsToHasValue.ContainsKey(options))
                _optionsToHasValue[options] = isDefault;
            else
                _optionsToHasValue.Add(new EquatableWeakReference(options), isDefault);
        }
    }
}
