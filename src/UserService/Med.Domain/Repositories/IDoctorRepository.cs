using Med.Domain.Entites;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task AddAsync(Doctor doctor);
    }
}
