using Med.Domain.Entities;

namespace Med.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
