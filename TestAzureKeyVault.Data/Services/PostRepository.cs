using TestAzureKeyVault.Shared.Contracts;
using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Data.Services;
public class PostRepository : GeneralRepository<Post>,IPostRepository
{
    public PostRepository(TestDbContext db):base(db)
    {
        Db = db;
    }

    public TestDbContext Db { get; }
}
