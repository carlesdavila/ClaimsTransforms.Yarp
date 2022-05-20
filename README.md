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
    Match = new RouteMatch
    {
        Path = "/profile"
    }
}.WithTransformAppendClaim(ClaimTypes.NameIdentifier)
```