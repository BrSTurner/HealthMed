using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class SpecialityRepository(UserContext context) : BaseRepository<Speciality>(context), ISpecialityRepository
    {
        public async Task<Speciality?> GetDoctorsBySpeciality(Guid id)
        {
            return await _entity.Include(x => x.Doctors).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
