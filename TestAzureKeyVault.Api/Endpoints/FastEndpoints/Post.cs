using FastEndpoints;
using Microsoft.Identity.Web;
using System.Data;
using TestAzureKeyVault.Shared.Contracts;
using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Api.Endpoints.FastEndpoints;


public class Post : Endpoint<EmptyRequest, List<Response>, Mapper>
{
    private readonly IPostRepository _repository;

    public Post(IPostRepository repository)
    {
        _repository = repository;
    }
    public override void Configure()
    {
        Get("api/fast/post");

        Options(b => b
            .RequireScope(Authorization.Scopes.ApiReadWrite)
            .RequireAuthorization(Authorization.Policies.AdminsGroup, Authorization.Policies.AppEditors));        
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        Console.WriteLine(HttpContext.User?.Identity?.Name);
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