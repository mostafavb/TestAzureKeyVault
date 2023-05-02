using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TestAzureKeyVault.Shared.Models;
using TestAzureKeyVault.Shared.Services;
using TestAzureKeyVault.Ui.Models;


namespace TestAzureKeyVault.Ui.Infrastructures;

public static class ConfigExtension
{

    public static async Task ImportConfiguration(this WebAssemblyHostBuilder builder, string api)
    {
        try
        {
            var sp = builder.Services.BuildServiceProvider();
            var crypto = sp.GetRequiredService<ICrypto>();

            if (crypto is not null)
            {                
                using var http = new HttpClient
                {
                    BaseAddress = new Uri(api)
                };
                
                var recivedString = await http.GetFromJsonAsync<string>("/config/appsettingsencrypted/Setted");

                string firstDecryptedString = crypto.DecryptByKeyInternal(recivedString!, "EmptyKey");
                
                var recivedByte = Convert.FromBase64String(firstDecryptedString!);

                var recivedConfig = JsonSerializer.Deserialize<ClientConfiguration>(recivedByte!);
                
                var azAd = recivedConfig?.AzureAd;
                var stringRecivedByte = Encoding.UTF8.GetString(azAd!.ApiSignature);
                var base64Key = Encoding.UTF8.GetString(Convert.FromBase64String(stringRecivedByte));
                var key = Encoding.UTF8.GetString(Convert.FromBase64String(base64Key));

                var config = new UiClientConfiguration
                {
                    AzureAd = new UiAzureAdSettings
                    {
                        Authority = crypto.DecryptByKeyInternal(Encoding.UTF8.GetString(azAd.Authority!), key),
                        ClientId = crypto.DecryptByKeyInternal(Encoding.UTF8.GetString(azAd.ClientId!), key),
                        Scope = crypto.DecryptByKeyInternal(Encoding.UTF8.GetString(azAd.Scope!), key),
                        ValidateAuthority = azAd.ValidateAuthority,
                    }
                };
                
                using Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(config)));

                
                var configuration = new ConfigurationBuilder()
                           .AddJsonStream(stream)
                           .Build();

                builder.Configuration.AddConfiguration(configuration);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }    
}
