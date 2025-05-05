namespace Med.SharedKernel.DomainObjects
{
    public abstract class Entity : CustomEntity<Guid>
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
        }
    }
}
