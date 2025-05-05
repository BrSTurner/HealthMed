using Med.Domain.Entites;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<Doctor?> GetDoctorByCRM(string crm);
        Task AddAsync(Doctor doctor);
    }
}
