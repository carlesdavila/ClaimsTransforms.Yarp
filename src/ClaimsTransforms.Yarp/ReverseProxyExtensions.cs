﻿using Microsoft.Extensions.DependencyInjection;

namespace ClaimsTransforms.Yarp;

public static class ReverseProxyExtensions
{
    public static IReverseProxyBuilder AddClaimsTransforms(this IReverseProxyBuilder builder)
    {
        builder.AddTransformFactory<ClaimsTransformFactory>();
        return builder;
    }
}