namespace TestAzureKeyVault.Shared.Models;
public class Post : BaseClass
{
    public string Title { get; set; }   
    public string Content { get; set; }   
    public int CategoryId { get; set; }
    public virtual Category Category { get; set; }  
}
