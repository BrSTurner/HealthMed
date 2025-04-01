using Med.Domain.Entites;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task AddAsync(Patient doctor);
    }
}
