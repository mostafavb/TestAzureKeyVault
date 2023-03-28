using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using TestAzureKeyVault.Shared.Contracts;

namespace TestAzureKeyVault.Api.Endpoints.Post;

public static class PostMapGroup
{
    public static RouteGroupBuilder MapPosts(this RouteGroupBuilder group)
    {
        group.MapGet("/", [Authorize(Roles = "Manager"), RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")] async (IPostRepository repository) =>
        {
            var posts = await repository.GetAll();
            return posts;
        });

        group.MapGet("/{id:int}", async (int id, IPostRepository repository) =>
        {
            var posts = await repository.Get(id);
            return posts;
        });


        return group;
    }
}
