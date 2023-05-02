using Microsoft.AspNetCore.Authorization;

namespace TestAzureKeyVault.Api.Configurations;

public class GroupAuthorizationRequirement : IAuthorizationRequirement
{
    public GroupAuthorizationRequirement(string groupId)
    {
        GroupId = groupId;
    }

    public string GroupId { get; }
}
