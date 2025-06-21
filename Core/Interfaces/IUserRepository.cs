namespace Core.Interfaces
{
    public interface IUserRepository<TEntity> : IRepository<TEntity>
    {
        Task<TEntity> GetByEmail(string email);

        Task<TEntity> GetByIdWithTrack(int id);
    }
}
