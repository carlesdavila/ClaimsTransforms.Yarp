using System.Security.Claims;
using ClaimsTransforms.Yarp;
using Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromMemory(YarpConfig.Routes, YarpConfig.Clusters)
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddClaimsTransforms();

builder.Services.AddAuthorization();
    
var app = builder.Build();

//All request are authenticated
app.Use(async (context, next) =>
{
    context.User.AddIdentity(new ClaimsIdentity(new Claim[]
    {
        new (ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
        new("user-id", "112233"),
        new("tenant-id", "1234")
    }, "mock"));
    await next();
});

app.MapGet("/", () => "Hello World!");

app.UseAuthorization();

app.MapReverseProxy();

app.Run();