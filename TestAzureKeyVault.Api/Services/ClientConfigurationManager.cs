using System.Text;
using System.Text.Json;
using TestAzureKeyVault.Shared.Models;
using TestAzureKeyVault.Shared.Services;

namespace TestAzureKeyVault.Api.Services;
public sealed class ClientConfigurationManager : IClientConfigurationManager
{
    private readonly Dictionary<string, ClientConfiguration> _clients = new();


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
        var recivedByte = Encoding.UTF8.GetBytes(Convert.ToBase64String(string64ToByte));
        var stringRecivedByte = Encoding.UTF8.GetString(recivedByte);
        
        var byte1 = Convert.FromBase64String(stringRecivedByte);
        var string1 = Encoding.UTF8.GetString(byte1);

        var byte2 = Convert.FromBase64String(string1);
        var string2 = Encoding.UTF8.GetString(byte2);

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
            return await Task.FromResult(Convert.ToBase64String(serialized));
        }
        return "Nothing found for this config!";
       
    }   

}