using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entites
{
    public abstract class User : Entity
    {
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
    }
}
