using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using MaSch.Core;

namespace MaSch.Presentation.Avalonia.Converter;

/// <summary>
/// Value converter that performs arithmetic calculations over its argument(s).
/// </summary>
/// <remarks>
/// MathConverter can act as a value converter, or as a multivalue converter (WPF only).
/// It is also a markup extension (WPF only) which allows to avoid declaring resources,
/// ConverterParameter must contain an arithmetic expression over converter arguments. Operations supported are +, -, * and /
/// Single argument of a value converter may referred as x, a, or {0}
/// Arguments of multi value converter may be referred as x,y,z,t (first-fourth argument), or a,b,c,d, or {0}, {1}, {2}, {3}, {4}, ...
/// The converter supports arithmetic expressions of arbitrary complexity, including nested subexpressions.
/// This Converter has been copied from: http://www.codeproject.com/Articles/239251/MathConverter-How-to-Do-Math-in-XAML.
/// </remarks>
public class MathConverter : MarkupExtension, IMultiValueConverter, IValueConverter
{
    private readonly Dictionary<string, IExpression> _storedExpressions = new();

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "Private interfaces do not need to be documented.")]
    private interface IExpression
    {
        decimal Eval(IList<object> args);
    }

    /// <inheritdoc />
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Convert(new[] { value }, targetType, parameter, culture);
    }

    /// <inheritdoc />
    public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var result = Parse(Guard.NotNull(parameter.ToString(), nameof(parameter))).Eval(values);
            if (targetType == typeof(decimal))
                return result;
            if (targetType == typeof(string))
                return result.ToString(CultureInfo.InvariantCulture);
            if (targetType == typeof(int))
                return (int)result;
            if (targetType == typeof(double) || targetType == typeof(object))
                return (double)result;
            if (targetType == typeof(long))
                return (long)result;
            throw new ArgumentException($"Unsupported target type {targetType.FullName}");
        }
        catch (Exception ex)
        {
            ProcessException(ex);
        }

        return AvaloniaProperty.UnsetValue;
    }

    /// <inheritdoc />
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    /// <summary>
    /// Handles exceptions that occure during conversion.
    /// </summary>
    /// <param name="ex">The exception.</param>
    protected virtual void ProcessException(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    private IExpression Parse(string s)
    {
        if (!_storedExpressions.TryGetValue(s, out IExpression? result))
        {
            result = new Parser().Parse(s);
            _storedExpressions[s] = result;
        }

        return result;
    }

    private sealed class Constant : IExpression
    {
        private readonly decimal _value;

        public Constant(string text)
        {
            if (!decimal.TryParse(text, out _value))
            {
                throw new ArgumentException($"'{text}' is not a valid number");
            }
        }

        public decimal Eval(IList<object> args)
        {
            return _value;
        }
    }

    private sealed class Variable : IExpression
    {
        private readonly int _index;

        public Variable(string text)
        {
            if (!int.TryParse(text, out _index) || _index < 0)
            {
                throw new ArgumentException($"'{text}' is not a valid parameter index");
            }
        }

        public Variable(int n)
        {
            _index = n;
        }

        public decimal Eval(IList<object> args)
        {
            if (_index >= args.Count)
            {
                throw new ArgumentException(
                    $"MathConverter: parameter index {_index} is out of range. {args.Count} parameter(s) supplied");
            }

            return System.Convert.ToDecimal(args[_index]);
        }
    }

    private sealed class BinaryOperation : IExpression
    {
        private readonly Func<decimal, decimal, decimal> _operation;
        private readonly IExpression _left;
        private readonly IExpression _right;

        public BinaryOperation(char operation, IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
            _operation = operation switch
            {
                '+' => (a, b) => (a + b),
                '-' => (a, b) => (a - b),
                '*' => (a, b) => (a * b),
                '/' => (a, b) => (a / b),
                _ => throw new ArgumentException($"Invalid operation {operation}"),
            };
        }

        public decimal Eval(IList<object> args)
        {
            return _operation(_left.Eval(args), _right.Eval(args));
        }
    }

    private sealed class Negate : IExpression
    {
        private readonly IExpression _param;

        public Negate(IExpression param)
        {
            _param = param;
        }

        public decimal Eval(IList<object> args)
        {
            return -_param.Eval(args);
        }
    }

    private sealed class Parser
    {
        private string? _text;
        private int _pos;

        public IExpression Parse(string text)
        {
            try
            {
                _pos = 0;
                _text = text;
                var result = ParseExpression();
                RequireEndOfText();
                return result;
            }
            catch (Exception ex)
            {
                string msg =
                    $"MathConverter: error parsing expression '{text}'. {ex.Message} at position {_pos}";

                throw new ArgumentException(msg, ex);
            }
        }

        private IExpression ParseExpression()
        {
            IExpression left = ParseTerm();

            while (true)
            {
                if (_pos >= _text!.Length)
                    return left;

                var c = _text[_pos];

                if (c == '+' || c == '-')
                {
                    ++_pos;
                    IExpression right = ParseTerm();
                    left = new BinaryOperation(c, left, right);
                }
                else
                {
                    return left;
                }
            }
        }

        private IExpression ParseTerm()
        {
            IExpression left = ParseFactor();

            while (true)
            {
                if (_pos >= _text!.Length)
                    return left;

                var c = _text[_pos];

                if (c == '*' || c == '/')
                {
                    ++_pos;
                    IExpression right = ParseFactor();
                    left = new BinaryOperation(c, left, right);
                }
                else
                {
                    return left;
                }
            }
        }

        private IExpression ParseFactor()
        {
            SkipWhiteSpace();
            if (_pos >= _text!.Length)
                throw new ArgumentException("Unexpected end of text");

            var c = _text[_pos];

            if (c == '+')
            {
                ++_pos;
                return ParseFactor();
            }

            if (c == '-')
            {
                ++_pos;
                return new Negate(ParseFactor());
            }

            if (c == 'x' || c == 'a')
                return CreateVariable(0);
            if (c == 'y' || c == 'b')
                return CreateVariable(1);
            if (c == 'z' || c == 'c')
                return CreateVariable(2);
            if (c == 't' || c == 'd')
                return CreateVariable(3);

            if (c == '(')
            {
                ++_pos;
                var expression = ParseExpression();
                SkipWhiteSpace();
                Require(')');
                SkipWhiteSpace();
                return expression;
            }

            if (c == '{')
            {
                ++_pos;
                var end = _text.IndexOf('}', _pos);
                if (end < 0)
                {
                    --_pos;
                    throw new ArgumentException("Unmatched '{'");
                }

                if (end == _pos)
                {
                    throw new ArgumentException("Missing parameter index after '{'");
                }

                var result = new Variable(_text[_pos..end].Trim());
                _pos = end + 1;
                SkipWhiteSpace();
                return result;
            }

            const string decimalRegEx = @"(\d+\.?\d*|\d*\.?\d+)";
            var match = Regex.Match(_text[_pos..], decimalRegEx);
            if (match.Success)
            {
                _pos += match.Length;
                SkipWhiteSpace();
                return new Constant(match.Value);
            }
            else
            {
                throw new ArgumentException($"Unexpeted character '{c}'");
            }
        }

        private IExpression CreateVariable(int n)
        {
            ++_pos;
            SkipWhiteSpace();
            return new Variable(n);
        }

        private void SkipWhiteSpace()
        {
            while (_pos < _text!.Length && char.IsWhiteSpace(_text[_pos]))
                ++_pos;
        }

        private void Require(char c)
        {
            if (_pos >= _text!.Length || _text[_pos] != c)
            {
                throw new ArgumentException($"Expected '{c}'");
            }

            ++_pos;
        }

        private void RequireEndOfText()
        {
            if (_pos != _text!.Length)
            {
                throw new ArgumentException($"Unexpected character '{_text[_pos]}'");
            }
        }
    }
}
