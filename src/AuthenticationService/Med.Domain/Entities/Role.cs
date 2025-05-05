using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class Role : CustomEntity<int>, IAggregateRoot
    {
        public required string Name { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
