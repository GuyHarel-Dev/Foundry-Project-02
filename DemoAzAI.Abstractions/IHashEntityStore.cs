namespace DemoAzAI.Abstractions
{
    public interface IHashEntityStore<TEntity>
    {
        Task<TEntity?> GetByHashAsync(string hash);

        Task SaveAsync(string hash, TEntity result);

        Task<bool> ExistsAsync(string hash);
    }
}
