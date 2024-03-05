using MedicineMarketPlace.BuildingBlocks.EntityFramework.Context;
using MedicineMarketPlace.BuildingBlocks.EntityFramework.Entities;
using MedicineMarketPlace.BuildingBlocks.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections;

namespace MedicineMarketPlace.BuildingBlocks.EntityFramework.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork where TContext : EfDbContext
    {
        private readonly TContext _context;
        private Hashtable _repositories;
        private readonly IDbContextTransaction _dbContextTransaction;

        public UnitOfWork(TContext context)
        {
            _context = context;
            _dbContextTransaction = _context.Database.CurrentTransaction;
        }

        public async Task CommitAsync()
        {
            await _dbContextTransaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _dbContextTransaction.RollbackAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IEfRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(EfRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IEfRepository<TEntity>)_repositories[type];
        }

        public IEfRepository<TEntity, TPrimaryKey> Repository<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            if (_repositories == null) _repositories = new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(EfRepository<,>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity), typeof(TPrimaryKey)), _context);

                _repositories.Add(type, repositoryInstance);
            }

            return (IEfRepository<TEntity, TPrimaryKey>)_repositories[type];
        }
    }
}
