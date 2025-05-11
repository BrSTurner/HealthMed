using Med.Application.Models.Dtos;
using Med.Application.Models.Inputs;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Models;

namespace Med.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<DomainResult> CreateUser(CreateUserInput input);

        Task<DoctorDTO?> GetDoctorByCrm(CRM crm);

        Task<PatientDTO?> GetPatientByCpf(CPF cpf);

        Task<DoctorDTO?> GetDoctorById(Guid id);

        Task<PatientDTO?> GetPatientById(Guid id);
    }
}
