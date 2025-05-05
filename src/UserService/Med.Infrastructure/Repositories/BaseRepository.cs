using Med.Infrastructure.Data;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
        protected readonly UserContext _context;
        protected readonly DbSet<TEntity> _entity;

        public BaseRepository(UserContext context)
        {
            _context = context;
            _entity = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity) 
            => await _entity.AddAsync(entity);
    }
}
