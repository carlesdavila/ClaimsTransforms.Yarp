using Yarp.ReverseProxy.Transforms;

namespace ClaimsTransforms.Yarp;

public class AppendClaimTransform : RequestTransform
{
    public AppendClaimTransform(string claimType)
    {
        ClaimType = claimType;
    }

    internal string ClaimType { get; }

    public override ValueTask ApplyAsync(RequestTransformContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        context.Path = $"{context.Path}/{context.HttpContext.User.FindFirst(ClaimType)?.Value}";

        return default;
    }
}