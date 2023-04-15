using System.Text.Json.Serialization;

namespace TestAzureKeyVault.Ui.Models;

public class UiAzureAdSettings
{
    [JsonPropertyName("Authority")]
    public string? Authority { get; set; }

    [JsonPropertyName("ClientId")]
    public string? ClientId { get; set; }

    [JsonPropertyName("ValidateAuthority")]
    public bool ValidateAuthority { get; set; }

    [JsonPropertyName("Scope")]
    public string? Scope { get; set; }
}

public class UiClientConfiguration
{
    [JsonPropertyName("AzureAd")]
    public UiAzureAdSettings AzureAd { get; set; }

   
}
