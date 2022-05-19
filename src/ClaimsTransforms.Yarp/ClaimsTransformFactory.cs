using Yarp.ReverseProxy.Transforms.Builder;

namespace ClaimsTransforms.Yarp;

public class ClaimsTransformFactory : ITransformFactory
{
    internal static readonly string AppendClaimKey = "AppendClaim";

    public bool Validate(TransformRouteValidationContext context, IReadOnlyDictionary<string, string> transformValues)
    {
        if (transformValues.TryGetValue(AppendClaimKey, out var value))
        {
            if (string.IsNullOrEmpty(value))
            {
                context.Errors.Add(new ArgumentException(
                    "A non-empty AppendClaim value is required"));
            }

            return true; // Matched
        }
        return false;
    }

    public bool Build(TransformBuilderContext context, IReadOnlyDictionary<string, string> transformValues)
    {
        if (transformValues.TryGetValue(AppendClaimKey, out var claimType))
        {
            context.AddAppendClaim(claimType);

            return true; // Matched
        }
        return false;
    }
}