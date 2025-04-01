using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Enumerations;

namespace Med.Domain.Entities
{
    public class User : Entity, IAggregateRoot
    {
        public required string Username { get; set; }
        public Email? Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserType Type { get; set; }
        public ICollection<UserRole>? Roles { get; set; }
    }
}
