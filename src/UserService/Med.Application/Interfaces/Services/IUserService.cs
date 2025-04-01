using Med.Application.Models.Inputs;
using Med.SharedKernel.Models;

namespace Med.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<DomainResult> CreateUser(CreateUserInput input);
    }
}
