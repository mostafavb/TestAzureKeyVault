using Microsoft.EntityFrameworkCore;
using TestAzureKeyVault.Shared.Contracts;
using TestAzureKeyVault.Shared.Models;

namespace TestAzureKeyVault.Data.Services;
public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseClass
{
    public GeneralRepository(TestDbContext db)
    {
        Db = db;
    }

    public TestDbContext Db { get; }

    public async Task<int> Add(T t)
    {
        Db.Add(t);
        await Db.SaveChangesAsync();
        return t.Id;
    }

    public Task Delete(T t)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Get(int id) => await Db.Set<T>().FirstOrDefaultAsync(f => f.Id == id);

    public async Task<List<T>> GetAll()
    {
        var lst = await Db.Set<T>().ToListAsync();
        return lst;
    }

    public Task Update(T t)
    {
        throw new NotImplementedException();
    }
}
