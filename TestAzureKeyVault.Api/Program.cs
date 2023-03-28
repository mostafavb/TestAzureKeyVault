using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using TestAzureKeyVault.Api.Endpoints;
using TestAzureKeyVault.Data;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Configuration.AddAzureKeyVault(
    new Uri(config["KeyVault:URL"]!),
    new DefaultAzureCredential(),
    new KeyVaultSecretManager()
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(config);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger Azure AD Demo", Version = "v1" });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Oauth2.0 wich uses AuthorizationCode flow",
        Name = "oauth2.0",
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(config["SwaggerAzureAd:AuthorizationUrl"]!),
                TokenUrl = new Uri(config["SwaggerAzureAd:TokenUrl"]!),
                Scopes = new Dictionary<string, string>
                {
                    { config["SwaggerAzureAd:Scope"]!,"Access API as User"}
                }
            }
        }
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new[]{ config["SwaggerAzureAd:Scope"] }
        }
    });
});
builder.Services.DataServicesRegistration(builder.Configuration);

var app = builder.Build();

app.RegisterEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(config["SwaggerAzureAd:ClientId"]);
        c.OAuthUsePkce();
        c.OAuthScopeSeparator(" ");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
