﻿using System.Text;
using System.Text.Json;
using TestAzureKeyVault.Shared.Models;
using TestAzureKeyVault.Shared.Services;

namespace TestAzureKeyVault.Api.Services;
public sealed class ClientConfigurationManager : IClientConfigurationManager
{
    private readonly Dictionary<string, ClientConfiguration> _clients = new();
    private readonly ICrypto _crypto;

    public ClientConfigurationManager(IConfiguration configuration, IServiceProvider services, ICrypto crypto)
    {

        var key = configuration.GetValue<string>("ApiSignature");
        var keyTo64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(key!));
        var string64ToByte = Encoding.UTF8.GetBytes(keyTo64String);

        var clientTypes = configuration.GetSection("ClientConfiguration");
        foreach (var clientType in clientTypes.GetChildren())
        {
            _clients[clientType.Key] = new ClientConfiguration()
            {
                AzureAd = new()
                {
                    Authority = Encoding.UTF8.GetBytes(crypto.EncryptByKeyInternal(clientType.GetValue<string>("Authority")!, key!) ?? ""),
                    ClientId = Encoding.UTF8.GetBytes(crypto.EncryptByKeyInternal(clientType.GetValue<string>("ClientId")!, key!) ?? ""),
                    ValidateAuthority = true,
                    Scope = Encoding.UTF8.GetBytes(crypto.EncryptByKeyInternal(clientType.GetValue<string>("Scope")!, key!) ?? ""),
                    ApiSignature = Encoding.UTF8.GetBytes(Convert.ToBase64String(string64ToByte))

                }
            };
        }
        _crypto = crypto;
    }

    public async Task<ClientConfiguration> GetClientConfiguration(string? clientType)
    {
        if (clientType != null && _clients.ContainsKey(clientType))
        {
            var c = _clients[clientType] ?? new();
            
            return await Task.FromResult(c);
        }
        return null;
    }

    public async Task<string> GetEncryptedClientConfiguration(string? clientType)
    {
        if (clientType != null && _clients.ContainsKey(clientType))
        {
            var c = _clients[clientType] ?? new();
            var serialized = JsonSerializer.SerializeToUtf8Bytes(c);
            return await Task.FromResult(_crypto.EncryptByKeyInternal(Convert.ToBase64String(serialized),"EmptyKey"));
        }
        return "Nothing found for this config!";
       
    }   

}