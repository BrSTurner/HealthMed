using Med.Application.Interfaces.Services;

namespace Med.Application.Services
{
    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public bool IsPasswordValid(string password, string hashedPassword)
            => BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
