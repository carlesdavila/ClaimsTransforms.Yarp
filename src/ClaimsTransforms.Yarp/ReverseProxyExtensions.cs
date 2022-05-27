using ClaimsTransforms.Yarp;

namespace Microsoft.Extensions.DependencyInjection;

public static class ReverseProxyExtensions
{
    public static IReverseProxyBuilder AddClaimsTransforms(this IReverseProxyBuilder builder)
    {
        builder.AddTransformFactory<ClaimsTransformFactory>();
        return builder;
    }
}