using MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities;
using MedicineMarketPlace.BuildingBlocks.EntityFramework.Repositories;

namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEfRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
        IEfRepository<TEntity, TPrimaryKey> Repository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>;
        Task CommitAsync();
        Task RollbackAsync();
    }
}
