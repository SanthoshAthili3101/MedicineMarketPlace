using MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities;
using MedicineMarketPlace.BuildingBlocks.EntityFramework.Specifications;

namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.Repositories
{
    public interface IEfRepository<TEntity> where TEntity : IEntity
    {
        Task<IReadOnlyList<TEntity>> FindAsync();

        Task<IReadOnlyList<TEntity>> FindWithSpecAsync(ISpecification<TEntity> spec);

        Task<TEntity> FindEntityWithSpec(ISpecification<TEntity> spec);

        Task<int> CountAsync(ISpecification<TEntity> spec);

        Task CreateWithoutTrackAsync(TEntity entity);

        Task CreateAsync(TEntity entity);

        Task CreateWithoutTrackAsync(List<TEntity> entities);

        Task CreateAsync(List<TEntity> entities);

        Task UpdateAsync(TEntity entity);

        Task UpdateAsync(List<TEntity> entities);

        Task DeleteAsync(TEntity entity);

        Task DeleteAsync(List<TEntity> entities);
    }

    public interface IEfRepository<TEntity, TPrimaryKey> : IEfRepository<TEntity> where TEntity : IEntity<TPrimaryKey>
    {
        Task<TEntity> FindByIdAsync(TPrimaryKey id);

        Task<List<TEntity>> FindByIdsAsync(List<TPrimaryKey> ids);

        Task UpdateWithoutTrackAsync(TEntity entity);

        new Task UpdateAsync(TEntity entity);

        new Task UpdateAsync(List<TEntity> entities);

        Task DeleteByIdAsync(TPrimaryKey id);

        Task DeleteByIdsAsync(List<TPrimaryKey> ids);
    }
}
