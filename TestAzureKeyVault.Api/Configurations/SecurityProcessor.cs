using FastEndpoints;
using FluentValidation.Results;

namespace TestAzureKeyVault.Api.Configurations;

public class SecurityProcessor<Treq,Tresp> : IPreProcessor<EmptyRequest>
{

    public Task PreProcessAsync(EmptyRequest req, HttpContext ctx, List<ValidationFailure> failures, CancellationToken ct)
    {
        var tenantID = ctx.Request.Headers["tenant-id"].FirstOrDefault();

        if (tenantID == null)
        {
            failures.Add(new("MissingHeaders", "The [tenant-id] header needs to be set!"));
            return ctx.Response.SendErrorsAsync(failures); //sending response here
        }

        if (tenantID != "qwerty")
            return ctx.Response.SendForbiddenAsync(); //sending response here

        return Task.CompletedTask;
    }

}
