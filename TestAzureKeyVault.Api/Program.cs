global using TestAzureKeyVault.Shared.Constants;

using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using TestAzureKeyVault.Api.Configurations;
using TestAzureKeyVault.Api.Endpoints;
using TestAzureKeyVault.Api.Services;
using TestAzureKeyVault.Data;
using TestAzureKeyVault.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

config.AddAzureKeyVault(
    new Uri(config["KeyVault:URL"]!),
    new DefaultAzureCredential(),
    new KeyVaultSecretManager()
    );

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(config.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy(Authorization.Policies.AdminsGroup, policy =>
        policy.Requirements.Add(new GroupAuthorizationRequirement(config["AzureAdAppAuthorization:AppAdminsGroupId"]!)));

    opt.AddPolicy(Authorization.Policies.AppUsersGroup, policy =>
        policy.Requirements.Add(new GroupAuthorizationRequirement(config["AzureAdAppAuthorization:AppUsersGroupId"]!)));

    opt.AddPolicy(Authorization.Policies.AppEditors, policy =>
        policy.RequireRole(Authorization.Roles.Editor));
});

builder.Services.AddSingleton<IAuthorizationHandler, GroupAuthorizationHandler>();
builder.Services.AddSingleton<ICrypto, Crypto>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        policy =>
        {
            policy
                   
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            
        });
});

builder.Services.AddSingleton(typeof(IClientConfigurationManager), typeof(ClientConfigurationManager));

builder.Services.AddFastEndpoints();

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

builder.Services.DataServicesRegistration(config);

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
    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();


app.Run();
