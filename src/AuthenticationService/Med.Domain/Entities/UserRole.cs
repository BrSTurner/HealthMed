using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class UserRole : Entity
    {
        public Guid UserId { get; set; }
        public required User User { get; set; }
        public Guid RoleId { get; set; }
        public required Role Role { get; set; }
    }
}
