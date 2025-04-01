using Med.SharedKernel.DomainObjects;

namespace Med.SharedKernel.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity, IAggregateRoot
    {
    }
}
