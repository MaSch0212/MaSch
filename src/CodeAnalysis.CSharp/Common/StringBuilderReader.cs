namespace MaSch.CodeAnalysis.CSharp.Common;

// https://github.com/dotnet/roslyn/blob/02436e52ce59da5d060fb3f5eda630c9205b67cf/src/Compilers/Core/Portable/Text/StringBuilderReader.cs
internal sealed class StringBuilderReader : TextReader
{
    private readonly StringBuilder _stringBuilder;
    private int _position;

    public StringBuilderReader(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
        _position = 0;
    }

    public override int Peek()
    {
        if (_position == _stringBuilder.Length)
            return -1;

        return _stringBuilder[_position];
    }

    public override int Read()
    {
        if (_position == _stringBuilder.Length)
            return -1;

        return _stringBuilder[_position++];
    }

    public override int Read(char[] buffer, int index, int count)
    {
        var length = Math.Min(count, _stringBuilder.Length - _position);
        _stringBuilder.CopyTo(_position, buffer, index, length);
        _position += length;
        return length;
    }

    public override int ReadBlock(char[] buffer, int index, int count) =>
        Read(buffer, index, count);

#if NETCOREAPP
    public override int Read(Span<char> buffer)
    {
        var length = Math.Min(buffer.Length, _stringBuilder.Length - _position);
        _stringBuilder.CopyTo(_position, buffer, length);
        _position += length;
        return length;
    }

    public override int ReadBlock(Span<char> buffer) =>
        Read(buffer);
#endif

    public override string ReadToEnd()
    {
        var result = _position == 0
            ? _stringBuilder.ToString()
            : _stringBuilder.ToString(_position, _stringBuilder.Length - _position);

        _position = _stringBuilder.Length;
        return result;
    }
}