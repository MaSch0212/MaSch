namespace MaSch.AspNetCore.Middlewares;

/// <summary>
/// Middleware that sets the base tag of any HTML document to the actual base path of the request.
/// </summary>
public class AdjustHtmlBaseLinkMiddleware
{
    private static readonly Regex BaseTagRegex = new(@"<base href=""(?<PathBase>[^""]+)""\s*\/?>", RegexOptions.Compiled);

    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="AdjustHtmlBaseLinkMiddleware"/> class.
    /// </summary>
    /// <param name="next">The request delegate that is used to continue to the next middleware.</param>
    public AdjustHtmlBaseLinkMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes this middleware asynchronously.
    /// </summary>
    /// <param name="context">The context of the HTTP request.</param>
    /// <returns>An awaitable task object.</returns>
    [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "This path delimiter can be hardcoded.")]
    public async Task InvokeAsync(HttpContext context)
    {
        var body = context.Response.Body;
        using var newBody = new MemoryStream();
        context.Response.Body = newBody;

        await _next(context);

        context.Response.Body = body;
        newBody.Seek(0, SeekOrigin.Begin);
        var pathBase = context.Request.PathBase.Value.TrimEnd('/') + '/';

        if (context.Response.ContentType == "text/html")
        {
            using var streamReader = new StreamReader(newBody);
            var html = await streamReader.ReadToEndAsync();
            var baseTagMatch = BaseTagRegex.Match(html);
            if (baseTagMatch.Success)
            {
                var pathBaseGroup = baseTagMatch.Groups["PathBase"];
                html = string.Concat(html[..pathBaseGroup.Index], pathBase, html[(pathBaseGroup.Index + pathBaseGroup.Value.Length)..]);
            }

            context.Response.ContentLength = null;
            await using (var sw = new StreamWriter(context.Response.Body))
            {
                await sw.WriteAsync(html);
            }
        }
        else
        {
            await newBody.CopyToAsync(context.Response.Body);
        }
    }
}
