using MaSch.Console.Cli.Configuration;
using MaSch.Core;
using MaSch.Core.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MaSch.Console.Cli.Runtime
{
    public class CliCommandOptionInfo : ICliCommandOptionInfo
    {
        private readonly PropertyInfo _property;

        public ICliCommandInfo Command { get; }
        public string PropertyName => _property.Name;
        public Type PropertyType => _property.PropertyType;
        public CliCommandOptionAttribute Attribute { get; }

        public IReadOnlyList<char> ShortAliases => Attribute.ShortAliases;
        public IReadOnlyList<string> Aliases => Attribute.Aliases;
        public object? DefaultValue => Attribute.Default;
        public bool IsRequired => Attribute.Required;
        public int HelpOrder => Attribute.HelpOrder;
        public string? HelpText => Attribute.HelpText;

        public CliCommandOptionInfo(ICliCommandInfo command, PropertyInfo property, CliCommandOptionAttribute attribute)
        {
            Command = Guard.NotNull(command, nameof(command));
            _property = Guard.NotNull(property, nameof(property));
            Attribute = Guard.NotNull(attribute, nameof(attribute));
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
    }
}
