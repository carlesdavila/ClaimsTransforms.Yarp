using Yarp.ReverseProxy.Transforms.Builder;

namespace ClaimsTransforms.Yarp;

internal sealed class ClaimsTransformFactory : ITransformFactory
{
    internal static readonly string AppendClaimKey = "AppendClaim";

    public bool Validate(TransformRouteValidationContext context, IReadOnlyDictionary<string, string> transformValues)
    {
        if (transformValues.TryGetValue(AppendClaimKey, out var value))
        {
            if (context.Route.AuthorizationPolicy is null)
            {
                context.Errors.Add(new ArgumentNullException(context.Route.AuthorizationPolicy,"AuthorizationPolicy value is required"));
            }
            
            if (string.IsNullOrEmpty(value))
            {
                context.Errors.Add(new ArgumentException("A non-empty AppendClaim value is required"));
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