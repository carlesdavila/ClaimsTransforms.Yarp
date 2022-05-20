using Sample;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromMemory(YarpConfig.Routes, YarpConfig.Clusters)
    .AddClaimsTransforms();

builder.Services.AddAuthorization();
    
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.UseAuthorization();

app.MapReverseProxy();

app.Run();