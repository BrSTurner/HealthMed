using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class Role : Entity, IAggregateRoot
    {
        public required string Name { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
