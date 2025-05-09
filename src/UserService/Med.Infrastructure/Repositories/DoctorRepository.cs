using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Med.SharedKernel.DomainObjects;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class DoctorRepository(UserContext context) : BaseRepository<Doctor>(context), IDoctorRepository
    {
        public async Task<Doctor?> GetDoctorByCRM(string crm)
            => await _entity.FirstOrDefaultAsync(p => p.CRM.Number == crm);

        public async Task<Doctor?> GetDoctorById(Guid id) 
            => await _entity.FirstOrDefaultAsync(p => p.UserId == id);
    }
}
