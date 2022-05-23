using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Transforms;

namespace ClaimsTransforms.Yarp;

public class ClaimsPrefixTransform : RequestTransform
{
    public ClaimsPrefixTransform(PathString claimTypePattern)
    {
        ClaimTypePattern = claimTypePattern;
    }
    
    internal PathString ClaimTypePattern { get; }

    public override ValueTask ApplyAsync(RequestTransformContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var resultPrefix = ClaimTypePattern.Value;
        var regex = new Regex(@"(?<=\{)(.*?)(?=\})");
        var matchGroups = regex.Matches(ClaimTypePattern.Value!);

        foreach (Match matchGroup in matchGroups)
        {
            var claimType = matchGroup.Value;
            var claimValue = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;

            if (claimValue != null)
            {
                resultPrefix = resultPrefix!.Replace($"{{{claimType}}}", claimValue);
            }
        }
        var prefixPath = new PathString(resultPrefix);
        context.Path = prefixPath + context.Path;

        return default;
    }
}