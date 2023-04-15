using FastEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web.Resource;
using System.Data;
using TestAzureKeyVault.Shared.Contracts;
using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Api.Endpoints.FastEndpoints;

//[Authorize(Roles = "Manager"), RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
public class Post : Endpoint<EmptyRequest, List<Response>, Mapper>
{
    private readonly IPostRepository _repository;

    public Post(IPostRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        AllowAnonymous();
        Get("api/fast/post");

    }
    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var posts = await _repository.GetAll();
        await SendAsync(Map.FromEntity(posts));
    }
}

public class Response
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public virtual Category Category { get; set; } = new();
}
public class Mapper : Mapper<EmptyRequest, List<Response>, List<Shared.Models.Post>>
{

    public override List<Response> FromEntity(List<Shared.Models.Post> e) =>
        e.Select(s => new Response
        {
            Title = s.Title,
            Content = s.Content,
            Category = s.Category,

        }).ToList();


}