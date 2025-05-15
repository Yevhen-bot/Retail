namespace Core.Interfaces
{
    public interface IUserRepository<TEntity> : IRepository<TEntity>
    {
        TEntity GetByEmail(string email);

        TEntity GetByIdWithTrack(int id);
    }
}
