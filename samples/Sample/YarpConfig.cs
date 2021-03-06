using System.Security.Claims;
using ClaimsTransforms.Yarp;
using Yarp.ReverseProxy.Configuration;
using Yarp.ReverseProxy.Transforms;

namespace Sample;

public static class YarpConfig
{
    public static RouteConfig[] Routes => new[]
    {
        new RouteConfig
            {
                AuthorizationPolicy = "default",
                RouteId = "Route1",
                ClusterId = "Cluster1",
                Match = new RouteMatch
                {
                    Path = "/profiles/me"
                }
            }
            .WithTransformPathSet("/users")
            .WithTransformAppendClaim(ClaimTypes.NameIdentifier),
        new RouteConfig
            {
                AuthorizationPolicy = "default",
                RouteId = "Route2",
                ClusterId = "Cluster1",
                Match = new RouteMatch
                {
                    Path = "/assets/{**catchall}"
                }
            }
            .WithTransformClaimsPrefix($"/tenants/{{tenant-id}}/users/{{{ClaimTypes.NameIdentifier}}}")
    };

    public static ClusterConfig[] Clusters => new[]
    {
        new ClusterConfig
        {
            ClusterId = "Cluster1",
            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { "destination1", new DestinationConfig { Address = "https://httpbin.org/anything/" } }
            }
        }
    };
}