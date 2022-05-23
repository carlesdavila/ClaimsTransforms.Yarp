using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Transforms;

namespace ClaimsTransforms.Yarp.Tests;

public class ClaimsPrefixTransformTests
{
    [Fact]
    public async Task ClaimsPrefixTransform_Success()
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new (ClaimTypes.Name, "someName"),
            new (ClaimTypes.NameIdentifier, "1"),
            new("some-claim-type", "claimValue")
        }, "mock"));
        
        var context = new RequestTransformContext()
        {
            HttpContext = new DefaultHttpContext() { User = user },
            Path = "/somePath"
        };
        
        var transform = new ClaimsPrefixTransform($"/{{{ClaimTypes.Name}}}/anotherpath/{{some-claim-type}}");
        
        // Act
        await transform.ApplyAsync(context);
        
        // Assert
        context.Path.ToString().Should().Be("/someName/anotherpath/claimValue/somePath");
    }
}