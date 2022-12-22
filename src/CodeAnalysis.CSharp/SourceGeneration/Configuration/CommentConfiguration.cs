namespace MaSch.CodeAnalysis.CSharp.SourceGeneration.Configuration;

/// <summary>
/// Represents configuration of a comment code element. This is used to generate code in the <see cref="ISourceBuilder"/>.
/// </summary>
[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Interface")]
public interface ICommentConfiguration : ICodeConfiguration
{
    /// <summary>
    /// Gets the comment type of the comment represented by this <see cref="ICommentConfiguration"/>.
    /// </summary>
    CommentType CommentType { get; }

    /// <summary>
    /// Gets the comment text of the comment represented by this <see cref="ICommentConfiguration"/>.
    /// </summary>
    string Comment { get; }
}

internal class CommentConfiguration : ICommentConfiguration
{
    public CommentConfiguration(CommentType commentType, string comment)
    {
        CommentType = commentType;
        Comment = comment;
    }

    public CommentType CommentType { get; }
    public string Comment { get; }

    public void WriteTo(ISourceBuilder sourceBuilder)
    {
        if (Comment is null or "")
            return;

        int lineNr = 0;
        int s = 0;
        int i;
        while ((i = Comment.IndexOf('\n', s)) >= 0)
        {
            AppendCommentLine(s, i - s, true);
            lineNr++;
            s = i + 1;
        }

        if (s < Comment.Length)
            AppendCommentLine(s, Comment.Length - s, lineNr > 0 || CommentType != CommentType.Block);
        if (CommentType == CommentType.Block)
            sourceBuilder.AppendLine(" */");

        void AppendCommentLine(int startIndex, int length, bool newline)
        {
            sourceBuilder.Append(
                CommentType switch
                {
                    CommentType.Block when lineNr == 0 => "/* ",
                    CommentType.Block => " * ",
                    CommentType.Doc => "/// ",
                    _ => "// ",
                });

            if (Comment[startIndex + length - 1] is '\r')
                length--;
            var line = Comment.Substring(startIndex, length);
            if (newline)
                sourceBuilder.AppendLine(line);
            else
                sourceBuilder.Append(line);
        }
    }
}
