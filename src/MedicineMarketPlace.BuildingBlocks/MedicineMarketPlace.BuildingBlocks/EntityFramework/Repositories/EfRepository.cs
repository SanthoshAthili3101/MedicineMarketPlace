using MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities;
using MedicineMarketPlace.BuildingBlocks.EntityFramework.Specifications;
using MedicineMarketPlace.BuildingBlocks.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.Repositories
{
    public class EfRepository<TEntity> : IEfRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbContext _context;

        public EfRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<TEntity>> FindAsync()
        {
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyList<TEntity>> FindWithSpecAsync(ISpecification<TEntity> spec)
        {
            var query = ApplySpecification(spec);
            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<TEntity> FindEntityWithSpec(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<TEntity> spec)
        {
            return await ApplySpecification(spec).AsNoTracking().CountAsync();
        }

        public async Task CreateWithoutTrackAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(TEntity entity)
        {
            SetTimestamp(entity);
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateWithoutTrackAsync(List<TEntity> entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(List<TEntity> entities)
        {
            entities.ForEach(_ => SetTimestamp(_));
            await _context.Set<TEntity>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(List<TEntity> entities)
        {
            //_context.Set<TEntity>().AttachRange(entities);
            //_context.Entry(entities).State = EntityState.Modified;

            _context.Set<TEntity>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.ChangeTracker.Clear();
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(List<TEntity> entities)
        {
            _context.ChangeTracker.Clear();
            _context.Set<TEntity>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            return SpecificationEvaluator<TEntity>.GetQuery(_context.Set<TEntity>().AsQueryable(), spec);
        }

        private void SetTimestamp(TEntity entity)
        {
            entity.CreatedDate = DateTime.UtcNow;

            foreach (var compositeEntity in entity.GetAllCompositeEntities().ToEmptyListIfNull())
            {
                compositeEntity.CreatedDate = DateTime.UtcNow;
            }
        }
    }

    public class EfRepository<TEntity, TPrimaryKey> : EfRepository<TEntity>, IEfRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly DbContext _context;

        public EfRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TEntity> FindByIdAsync(TPrimaryKey id)
        {
            return await _context.Set<TEntity>()
                .AsNoTracking()
                .Where(_ => Object.Equals(_.Id, id))
                .SingleOrDefaultAsync();
        }

        public async Task<List<TEntity>> FindByIdsAsync(List<TPrimaryKey> ids)
        {
            if (!ids.ToEmptyListIfNull().Any())
                return new List<TEntity>();

            var entities = await _context.Set<TEntity>()
                .AsNoTracking()
                .Where(_ => ids.Contains(_.Id))
                .ToListAsync();

            return entities;
        }

        public async Task UpdateWithoutTrackAsync(TEntity entity)
        {
            var existingEntity = await FindByIdAsync(entity.Id);

            if (existingEntity == null)
                throw new Exception($"{typeof(TEntity).Name} with id {entity.Id} does not exist");

            await base.UpdateAsync(entity);
        }

        public new async Task UpdateAsync(TEntity entity)
        {
            var existingEntity = await FindByIdAsync(entity.Id);

            if (entity.ShouldHandleConcurrency())
            {
                SetTimestampFromExistingEntity(entity, existingEntity);
                SetTimestampAndTrack(entity);
            }

            await base.UpdateAsync(entity);
        }

        public new async Task UpdateAsync(List<TEntity> entities)
        {
            var entityIds = entities.Select(_ => _.Id).ToList();
            var existingEntities = await FindByIdsAsync(entityIds);

            var existingEntityIds = existingEntities.Select(_ => _.Id).ToList();
            if (entityIds.Except(existingEntityIds).Any())
            {
                var idsWithComma = string.Join(",", entityIds.Except(existingEntityIds).Select(_ => _.ToString()));
                throw new Exception($"{typeof(TEntity).Name} with ids [{idsWithComma}] does not exist");
            }

            if (entities.First().ShouldHandleConcurrency())
            {
                foreach (var entity in entities)
                {
                    var existingEntity = existingEntities.Single(_ => _.Id.Equals(entity.Id));
                    SetTimestampFromExistingEntity(entity, existingEntity);
                    SetTimestampAndTrack(entity);
                }
            }

            await base.UpdateAsync(entities);
        }

        public async Task DeleteByIdAsync(TPrimaryKey id)
        {
            await DeleteAsync(await FindByIdAsync(id));
        }

        public async Task DeleteByIdsAsync(List<TPrimaryKey> ids)
        {
            await DeleteAsync(await FindByIdsAsync(ids));
        }

        private void SetTimestampFromExistingEntity(TEntity entityToUpdate, TEntity existingEntity)
        {
            entityToUpdate.CreatedBy = existingEntity.CreatedBy;
            entityToUpdate.CreatedDate = existingEntity.CreatedDate;
            var existingCompositeEntities = existingEntity.GetAllCompositeEntities().ToEmptyListIfNull();

            foreach (var compositeEntityToUpdate in entityToUpdate.GetAllCompositeEntities().ToEmptyListIfNull())
            {
                var existingCompositeEntity = existingCompositeEntities.FirstOrDefault(_ => _.GetIdentityValue().Equals(compositeEntityToUpdate.GetIdentityValue()) && _.GetType() == compositeEntityToUpdate.GetType());
                if (existingCompositeEntity != null)
                {
                    compositeEntityToUpdate.CreatedBy = existingCompositeEntity.CreatedBy;
                    compositeEntityToUpdate.CreatedDate = existingCompositeEntity.CreatedDate;
                }
                else
                {
                    compositeEntityToUpdate.CreatedDate = DateTime.UtcNow;
                }
            }
        }

        private void SetTimestampAndTrack(TEntity entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;

            foreach (var compositeEntity in entity.GetAllCompositeEntities().ToEmptyListIfNull())
            {
                compositeEntity.ModifiedDate = DateTime.UtcNow;
            }
        }
    }
}
