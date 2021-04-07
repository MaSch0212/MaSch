using MaSch.Core.Converters;

namespace MaSch.Core
{
    /// <summary>
    /// Represents a <see cref="IObjectConvertManager"/> that already contains common <see cref="IObjectConverter"/>s.
    /// </summary>
    /// <remarks>
    ///     Contains the following <see cref="IObjectConverter"/>s:
    ///     <see cref="NullableObjectConverter"/>,
    ///     <see cref="ConvertibleObjectConverter"/>,
    ///     <see cref="EnumConverter"/>,
    ///     <see cref="EnumerableConverter"/>.
    ///     <see cref="ToStringObjectConverter"/>,
    ///     <see cref="NullObjectConverter"/>,
    ///     <see cref="IdentityObjectConverter"/>.
    /// </remarks>
    /// <seealso cref="ObjectConvertManager" />
    public class DefaultObjectConvertManager : ObjectConvertManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultObjectConvertManager"/> class.
        /// </summary>
        public DefaultObjectConvertManager()
        {
            RegisterInitialConverters();
        }

        /// <summary>
        /// Registers the initial converters.
        /// </summary>
        protected virtual void RegisterInitialConverters()
        {
            RegisterConverter(new NullableObjectConverter());
            RegisterConverter(new ConvertibleObjectConverter());
            RegisterConverter(new EnumConverter());
            RegisterConverter(new EnumerableConverter());
            RegisterConverter(new ToStringObjectConverter(-98_000));
            RegisterConverter(new NullObjectConverter(-99_000));
            RegisterConverter(new IdentityObjectConverter(-100_000));
        }
    }
}
