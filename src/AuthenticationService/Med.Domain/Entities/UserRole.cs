using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class UserRole : Entity
    {
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public required int RoleId { get; set; }
        public virtual Role? Role { get; set; }
    }
}
