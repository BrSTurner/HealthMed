using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entities
{
    public class UserRole : Entity
    {
        public Guid UserId { get; set; }
        public  User? User { get; set; }
        public required int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
