# ClaimsTransforms.Yarp

_[![NuGet version](https://img.shields.io/nuget/v/ClaimsTransforms.Yarp)](https://www.nuget.org/packages/ClaimsTransforms.Yarp)_

Yarp Request Transforms using Claims.

### Installation

install ClaimsTransforms.Yarp from [NuGet](https://www.nuget.org/packages/ClaimsTransforms.Yarp):

    dotnet add package ClaimsTransforms.Yarp

### Setup

Add Claims Transforms to Yarp services
``` c#
builder.Services.AddReverseProxy()
.AddClaimsTransforms();
```

Adding transforms to routes
--------------------------------


### Append Claim Transform

Will append first value of claim type parameter to the end of the route.


Code:
``` c#
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
.WithTransformAppendClaim(ClaimTypes.NameIdentifier)
```

If ClaimsPrincipal contains 
 
``` c#   
 new Claim[] { new ("user-id", "1234") }
```

RouteConfig code matches ***/profiles/me*** and transforms to ***/users/1234***

### Claims Transform Prefix

Will include prefix to routes based on claims pattern. 
Claims pattern will replace values between { and } with Claim value.

Claims Prefix Pattern example:
   
     /route/{claim-type}


Code:
``` c#
new RouteConfig
{
    AuthorizationPolicy = "default",
    Match = new RouteMatch
    {
        Path = "/assets/{**catchall}"
    }
}
.WithTransformClaimsPrefix($"/tenants/{{tenant-id}}/users/{{{ClaimTypes.NameIdentifier}}}")
```

Example:

| **Step**         | **Value**                                                         |
|------------------|-------------------------------------------------------------------|
| Route definition | /assets/{**catchall}                                              |
| Request path     | /assets/action                                                    |
| Claims Pattern   | /tenants/{tenant-id}/users/{user-id}                              |
| Claims Principal | new Claim[] { new ("user-id", "1234"), new ("tenant-id", "abc") } |
| Result           | /tenants/abc/users/1234/assets/action                             |