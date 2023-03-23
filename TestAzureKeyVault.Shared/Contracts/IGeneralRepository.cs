namespace TestAzureKeyVault.Shared.Contracts;

public interface IGeneralRepository<T> where T : class
{
    Task<List<T>> GetAll();
    Task<T> Get(int id);
    Task<int> Add(T t);
    Task Delete(T t);
    Task Update(T t);
}
