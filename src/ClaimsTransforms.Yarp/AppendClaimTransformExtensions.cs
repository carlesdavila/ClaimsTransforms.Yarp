using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace ClaimsTransforms.Yarp;

public static class AppendClaimTransformExtensions
{
    internal static TransformBuilderContext AddAppendClaim(this TransformBuilderContext context, string claimType)
    {
        context.RequestTransforms.Add(new AppendClaimTransform(claimType));
        return context;
    }

    public static RouteConfig WithTransformAppendClaim(this RouteConfig routeConfig, string claimType)
    {
        return routeConfig.WithTransform(transform =>
        {
            transform[ClaimsTransformFactory.AppendClaimKey] = claimType;
        });
    }
}