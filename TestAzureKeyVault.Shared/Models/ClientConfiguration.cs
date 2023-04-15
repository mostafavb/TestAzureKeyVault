using System.Text.Json.Serialization;

namespace TestAzureKeyVault.Shared.Models;
public class AzureAdSettings
{
    [JsonPropertyName("Authority")]
    public byte[]? Authority { get; set; }

    [JsonPropertyName("ClientId")]
    public byte[]? ClientId { get; set; }

    [JsonPropertyName("ValidateAuthority")]
    public bool ValidateAuthority { get; set; }

    [JsonPropertyName("Scope")]
    public byte[]? Scope { get; set; }

    [JsonPropertyName("ApiSignature")]
    public byte[] ApiSignature { get; set; }

}

public class ClientConfiguration
{
    [JsonPropertyName("AzureAd")]
    public AzureAdSettings? AzureAd { get; set; }

}
