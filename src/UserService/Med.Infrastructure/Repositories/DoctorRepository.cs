using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class DoctorRepository(UserContext context) : BaseRepository<Doctor>(context), IDoctorRepository
    {
        public async Task<Doctor?> GetDoctorByCRM(string crm)
        {
           return  await _entity
                .Include(x => x.Speciality)
                .FirstOrDefaultAsync(p => p.CRM.Number == crm);
        }

        public async Task<Doctor?> GetDoctorById(Guid id)
        {
            return await _entity.Include(x => x.Speciality).FirstOrDefaultAsync(x => x.Id == id);
        }            
    }
}
