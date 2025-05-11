using Med.Domain.Entites;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface ISpecialityRepository : IRepository<Speciality>
    {
        Task<Speciality?> GetDoctorsBySpeciality(Guid id);
    }
}
