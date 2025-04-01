using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Enumerations;

namespace Med.MessageBus.Integration.Requests.Users
{
    public class CreateUserRequest
    {
        public string? Email { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public UserType Type { get; set; }
    }
}
