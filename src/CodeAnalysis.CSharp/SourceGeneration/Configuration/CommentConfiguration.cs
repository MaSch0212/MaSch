namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

internal class CommentConfiguration : ICodeConfiguration
{
    private readonly CommentType _commentType;
    private readonly string _comment;

    public CommentConfiguration(CommentType commentType, string comment)
    {
        _commentType = commentType;
        _comment = comment;
    }

    internal enum CommentType
    {
        Line,
        Block,
        Doc,
    }

    public void WriteTo(ISourceBuilder sourceBuilder)
    {
        if (_comment is null or "")
            return;

        int lineNr = 0;
        int s = 0;
        int i;
        while ((i = _comment.IndexOf('\n', s)) >= 0)
        {
            AppendCommentLine(s, i - s, true);
            lineNr++;
            s = i + 1;
        }

        if (s < _comment.Length)
            AppendCommentLine(s, _comment.Length - s, lineNr > 0 || _commentType != CommentType.Block);
        if (_commentType == CommentType.Block)
            sourceBuilder.AppendLine(" */");

        void AppendCommentLine(int startIndex, int length, bool newline)
        {
            sourceBuilder.Append(
                _commentType switch
                {
                    CommentType.Block when lineNr == 0 => "/* ",
                    CommentType.Block => " * ",
                    CommentType.Doc => "/// ",
                    _ => "// ",
                });

            if (_comment[startIndex + length - 1] is '\r')
                length--;
            var line = _comment.Substring(startIndex, length);
            if (newline)
                sourceBuilder.AppendLine(line);
            else
                sourceBuilder.Append(line);
        }
    }
}
