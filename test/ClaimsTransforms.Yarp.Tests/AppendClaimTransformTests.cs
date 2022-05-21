using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Transforms;

namespace ClaimsTransforms.Yarp.Tests;

public class AppendClaimTransformTests
{
    [Theory]
    [InlineData("/foo", ClaimTypes.Name, "someName", "/foo/someName")]
    [InlineData("/foo", ClaimTypes.NameIdentifier, "1", "/foo/1")]
    [InlineData("/foo", "custom-claim", "claimValue", "/foo/claimValue")]
    public async Task AppendClaimTransform_Success(string initialPath, string claimType, string claimValue, string expected)
    {
        // Arrange
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(claimType, claimValue)
        }, "mock"));
        
        var context = new RequestTransformContext()
        {
            HttpContext = new DefaultHttpContext() { User = user },
            Path = initialPath
        };
        var transform = new AppendClaimTransform(claimType);
        
        // Act
        await transform.ApplyAsync(context);
        
        // Assert
        context.Path.ToString().Should().Be(expected);
    }
}