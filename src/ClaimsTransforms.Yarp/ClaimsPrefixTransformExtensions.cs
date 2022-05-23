using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace ClaimsTransforms.Yarp;

public static class ClaimsPrefixTransformExtensions
{
    internal static TransformBuilderContext AddClaimsPrefix(this TransformBuilderContext context, string claimsTypesPattern)
    {
        context.RequestTransforms.Add(new ClaimsPrefixTransform(claimsTypesPattern));
        return context;
    }

    public static RouteConfig WithTransformClaimsPrefix(this RouteConfig routeConfig, string claimsTypesPattern)
    {
        return routeConfig.WithTransform(transform =>
        {
            transform[ClaimsTransformFactory.ClaimsPrefixKey] = claimsTypesPattern;
        });
    }
}