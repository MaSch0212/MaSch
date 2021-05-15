using MaSch.Console.Cli.Configuration;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    public class CliCommandValueInfo : ICliCommandValueInfo
    {
        private readonly PropertyInfo _property;

        public ICliCommandInfo Command { get; }
        public string PropertyName => _property.Name;
        public Type PropertyType => _property.PropertyType;
        public CliCommandValueAttribute Attribute { get; }

        public string DisplayName => Attribute.DisplayName;
        public int Order => Attribute.Order;
        public object? DefaultValue => Attribute.Default;
        public bool IsRequired => Attribute.Required;
        public string? HelpText => Attribute.HelpText;

        public CliCommandValueInfo(ICliCommandInfo command, PropertyInfo property, CliCommandValueAttribute attribute)
        {
            Command = command;
            _property = property;
            Attribute = attribute;
        }

        public void SetValue(object options, object? value)
        {
            if (typeof(IEnumerable).IsAssignableFrom(_property.PropertyType) && _property.PropertyType != typeof(string))
            {
                var currentValue = _property.GetValue(options);
                if (currentValue is IEnumerable e1 && value is IEnumerable e2)
                    value = e1.ToGeneric().Concat(e2.ToGeneric());
            }

            _property.SetValue(options, value.ConvertTo(_property.PropertyType));
        }

        public object? GetValue(object options)
            => _property.GetValue(options);
    }
}
