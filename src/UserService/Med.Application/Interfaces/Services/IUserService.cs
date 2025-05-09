using Med.Application.Models.Inputs;
using Med.Domain.Entites;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Models;

namespace Med.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<DomainResult> CreateUser(CreateUserInput input);

        Task<Doctor?> GetDoctorByCrm(CRM crm);
    }
}
