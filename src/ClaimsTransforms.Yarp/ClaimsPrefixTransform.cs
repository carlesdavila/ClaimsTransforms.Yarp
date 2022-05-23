using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Transforms;

namespace ClaimsTransforms.Yarp;

public class ClaimsPrefixTransform : RequestTransform
{
    public ClaimsPrefixTransform(string claimTypePattern)
    {
        ClaimTypePattern = claimTypePattern;
    }

    internal string ClaimTypePattern { get; }

    public override ValueTask ApplyAsync(RequestTransformContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var resultPrefix = ClaimTypePattern;
        var regex = new Regex(@"(?<=\{)(.*?)(?=\})");
        var matchGroups = regex.Matches(ClaimTypePattern);

        foreach (Match matchGroup in matchGroups)
        {
            var claimType = matchGroup.Value;
            var claimValue = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            if (claimValue != null) resultPrefix = resultPrefix!.Replace($"{{{claimType}}}", claimValue);
        }

        var prefixPath = MakePathString(resultPrefix);
        context.Path = prefixPath + context.Path;

        return default;
    }

    private static PathString MakePathString(string path)
    {
        ArgumentNullException.ThrowIfNull(path, nameof(path));

        if (!path.StartsWith("/", StringComparison.Ordinal)) path = "/" + path;
        return new PathString(path);
    }
}