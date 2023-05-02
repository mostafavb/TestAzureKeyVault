namespace TestAzureKeyVault.Shared.Constants;
public static class Authorization
{
    public static class Policies
    {
        public const string AdminsGroup = "AdminsGroupPolicy";
        public const string AppUsersGroup = "AppUsersGroupPolicy";
        public const string StaffGroup = "StaffGroupPolicy";
        public const string SuppliersGroup = "SuppliersGroupPolicy";
        public const string AppEditors = "AppEditors";


    }
    public static class Roles
    {
        public const string Editor = "app.edit";
        public const string Reader = "app.reader";
        public const string Creator = "app.create";
    }
    public static class Scopes
    {
        public const string ApiAccess = "api_access";
        public const string ApiRead = "api_read";
        public const string ApiReadWrite = "api_readwrite";
    }
}
