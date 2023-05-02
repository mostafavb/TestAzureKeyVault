using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using TestAzureKeyVault.Shared.Models;
using TestAzureKeyVault.Ui.Services;

namespace TestAzureKeyVault.Ui.Pages.Posts;

[AllowAnonymous]
public partial class Index
{

    [Inject] HttpClient Http { get; set; }
    [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

    public List<Post>? Responses { get; set; }
    bool hasPermissionDenied = false;


    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        if (user.Identity is not null && user.Identity.IsAuthenticated)
            try
            {
                HttpResponseMessage? res;
                var url = "api/fast/post";
                using var request = new HttpRequestMessage()
                {
                    Method = new HttpMethod("GET"),
                    RequestUri = new Uri(url, System.UriKind.RelativeOrAbsolute)
                };

                try
                {
                    res = await Http.SendAsync(request);

                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var headers_ = Enumerable.ToDictionary(res.Headers, h_ => h_.Key, h_ => h_.Value);
                        //if (res.Content != null && res.Content.Headers != null)
                        //{
                        //    foreach (var item_ in res.Content.Headers)
                        //        headers_[item_.Key] = item_.Value;
                        //}
                        Responses = await ClientService.ReadObjectResponseAsync<List<Post>>(res, headers_);
                    }
                    hasPermissionDenied = (res.StatusCode == System.Net.HttpStatusCode.Unauthorized || res.StatusCode == System.Net.HttpStatusCode.Forbidden);
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
            }
    }
}
