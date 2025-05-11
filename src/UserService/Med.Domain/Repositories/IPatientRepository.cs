using Med.Domain.Entites;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<Patient?> GetPatientByCPFAsync(string cpf);
        Task<Patient?> GetPatientByEmailAsync(string email);

        Task<Patient?> GetPatientByIdAsync(Guid id);
        Task AddAsync(Patient doctor);
    }
}
