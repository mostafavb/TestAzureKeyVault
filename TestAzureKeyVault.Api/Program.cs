using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using TestAzureKeyVault.Api.Endpoints;
using TestAzureKeyVault.Data;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Configuration.AddAzureKeyVault(
    new Uri(config["KeyVaultConfiguration:KeyVaultURL"]!),
    new DefaultAzureCredential(
        //new DefaultAzureCredentialOptions
        //{
        //    ManagedIdentityClientId = config["KeyVaultConfiguration:ClientId"]
        //}
        )
    , new KeyVaultSecretManager()
    );

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.DataServicesRegistration(builder.Configuration);

var app = builder.Build();

app.RegisterEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();
