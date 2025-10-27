using Microsoft.AspNetCore.Server.IISIntegration;

var builder = WebApplication.CreateBuilder();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddServiceModelServices()
    .AddServiceModelMetadata()
    .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();

builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
builder.Services.AddAuthorization(options => {
    options.AddPolicy("Deny",
        p => {
        p.RequireAssertion(_ => false);
    });
});
builder.Services.AddServiceModelServices();
builder.Services.AddServiceModelMetadata();
// builder.Services.AddServiceModelWebServices();
builder.Services.AddSingleton<Service>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseServiceModel(serviceBuilder =>
{
    serviceBuilder.AddService<Service>();
    var ep = serviceBuilder.AddServiceEndpoint<Service, IService>(
        new BasicHttpBinding
        {
            Security = new BasicHttpSecurity()
            {
                Mode = BasicHttpSecurityMode.Transport,
                Transport = new HttpTransportSecurity()
                {
                    ClientCredentialType = HttpClientCredentialType.InheritedFromHost,
                    AlwaysUseAuthorizationPolicySupport = true
                }
            }
        }, "/Service.svc");
    var serviceMetadataBehavior = app.Services.GetRequiredService<ServiceMetadataBehavior>();
    serviceMetadataBehavior.HttpsGetEnabled = true;
});

app.MapGet("/whoami", (HttpContext ctx) => new {
    user = ctx.User?.Identity?.Name,
    authenticated = ctx.User?.Identity?.IsAuthenticated ?? false
}).RequireAuthorization();

app.Run();
