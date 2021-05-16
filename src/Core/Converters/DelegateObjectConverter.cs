using System;

namespace MaSch.Core.Converters
{
    /// <summary>
    /// A <see cref="IObjectConverter"/> that is using a delegate to convert from the <typeparamref name="TSource"/>-type to the <typeparamref name="TTarget"/>-type.
    /// </summary>
    /// <typeparam name="TSource">The source type.</typeparam>
    /// <typeparam name="TTarget">The target type.</typeparam>
    public class DelegateObjectConverter<TSource, TTarget> : IObjectConverter
    {
        private readonly Func<TSource?, TTarget>? _converterFunction;
        private readonly Func<TSource?, IFormatProvider, TTarget>? _converterFunctionWithFormatProvider;
        private readonly int _basePriority;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateObjectConverter{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="converterFunction">The delegate that is used for the convertion.</param>
        /// <param name="priority">The priority of this <see cref="IObjectConverter"/>.</param>
        public DelegateObjectConverter(Func<TSource?, TTarget> converterFunction, int priority = 0)
        {
            _converterFunction = converterFunction;
            _basePriority = priority;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateObjectConverter{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="converterFunction">The delegate that is used for the convertion.</param>
        /// <param name="priority">The priority of this <see cref="IObjectConverter"/>.</param>
        public DelegateObjectConverter(Func<TSource?, IFormatProvider, TTarget> converterFunction, int priority = 0)
        {
            _converterFunctionWithFormatProvider = converterFunction;
            _basePriority = priority;
        }

        /// <inheritdoc />
        public bool CanConvert(Type? sourceType, Type targetType, IObjectConvertManager convertManager)
        {
            return ((sourceType == null && typeof(TSource).IsClass) || typeof(TSource).IsAssignableFrom(sourceType)) &&
                   typeof(TTarget).IsAssignableFrom(targetType);
        }

        /// <inheritdoc />
        public int GetPriority(Type? sourceType, Type targetType)
        {
            return _basePriority;
        }

        /// <inheritdoc />
        public object? Convert(object? obj, Type? sourceType, Type targetType, IObjectConvertManager convertManager, IFormatProvider formatProvider)
        {
            return _converterFunctionWithFormatProvider != null
                ? _converterFunctionWithFormatProvider((TSource?)obj, formatProvider)
                : _converterFunction != null
                    ? _converterFunction.Invoke((TSource?)obj)
                    : default;
        }
    }
}
