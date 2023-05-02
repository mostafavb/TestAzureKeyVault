using Microsoft.AspNetCore.Authorization;

namespace TestAzureKeyVault.Api.Configurations;

public class GroupAuthorizationHandler : AuthorizationHandler<GroupAuthorizationRequirement>
{
    //private readonly GraphServiceClient _graphServiceClient;

    //public GroupAuthorizationHandler(GraphServiceClient graphServiceClient)
    //{
    //    _graphServiceClient = graphServiceClient;
    //}

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirement requirement)
    {
        //var pendingRequirements = context.PendingRequirements.ToList();
        //foreach (var req in pendingRequirements)
        //{
        //    //if (req is ReadPermission)
        //    //{
        //    //}
        //}

        if (context.User.Identity!.IsAuthenticated)
        {
            if (context.User.Claims.Any(x => x.Type == "groups" && x.Value == requirement.GroupId))
            {
                context.Succeed(requirement);
            }
            //if (context.User.Claims.Any(x => x.Type == "hasgroup" && x.Value == "true"))
            //{
            //    var groupResponse = _graphServiceClient.Me.CheckMemberGroups.PostAsync(new Microsoft.Graph.Me.CheckMemberGroups.CheckMemberGroupsPostRequestBody { GroupIds = new List<string> { requirement.GroupId } }).Result;

            //    if (groupResponse.Value!.Any(x => x == requirement.GroupId))
            //        context.Succeed(requirement);

            //}
        }
        return Task.CompletedTask;
    }
}
