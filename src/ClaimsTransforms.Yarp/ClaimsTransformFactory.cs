using Yarp.ReverseProxy.Transforms.Builder;

namespace ClaimsTransforms.Yarp;

internal sealed class ClaimsTransformFactory : ITransformFactory
{
    internal static readonly string AppendClaimKey = "AppendClaim";
    internal static readonly string ClaimsPrefixKey = "ClaimsPrefix";

    public bool Validate(TransformRouteValidationContext context, IReadOnlyDictionary<string, string> transformValues)
    {
        if (transformValues.TryGetValue(AppendClaimKey, out var value))
        {
            if (context.Route.AuthorizationPolicy is null) context.Errors.Add(new ArgumentNullException(context.Route.AuthorizationPolicy, "AuthorizationPolicy value is required"));

            if (string.IsNullOrEmpty(value)) context.Errors.Add(new ArgumentException("A non-empty AppendClaim value is required"));
        }
        else if (transformValues.TryGetValue(ClaimsPrefixKey, out var claimsPathPrefix))
        {
            TransformHelpers.TryCheckTooManyParameters(context, transformValues, 1);
            CheckPathNotNull(context, ClaimsPrefixKey, claimsPathPrefix);
        }
        else
        {
            return false;
        }

        return true; // Matched
    }

    public bool Build(TransformBuilderContext context, IReadOnlyDictionary<string, string> transformValues)
    {
        if (transformValues.TryGetValue(AppendClaimKey, out var claimType))
        {
            context.AddAppendClaim(claimType);
        }
        else if (transformValues.TryGetValue(ClaimsPrefixKey, out var claimsPathPrefix))
        {
            TransformHelpers.CheckTooManyParameters(transformValues, 1);
            context.AddClaimsPrefix(claimsPathPrefix);
        }
        else
        {
            return false;
        }

        return true;
    }

    private void CheckPathNotNull(TransformRouteValidationContext context, string fieldName, string? path)
    {
        if (path is null) context.Errors.Add(new ArgumentNullException(fieldName));
    }
}