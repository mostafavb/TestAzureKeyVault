using Microsoft.EntityFrameworkCore;
using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Data;

public class TestDbContext : DbContext
{
    public virtual DbSet<Post> Posts { get; set; }
    public virtual DbSet<Category> Categories { get; set; }

    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
}