namespace TestAzureKeyVault.Shared.Models;
public class Category : BaseClass
{
    public string Title { get; set; }

    public virtual ICollection<Post> Posts { get; set; }
}
