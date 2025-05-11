using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Infrastructure.Repositories
{
    public class PatientRepository(UserContext context) : BaseRepository<Patient>(context), IPatientRepository
    {
        public async Task<Patient?> GetPatientByCPFAsync(string cpf)
            => await _entity.FirstOrDefaultAsync(p => p.CPF.Number == cpf);

        public async Task<Patient?> GetPatientByEmailAsync(string email)
            => await _entity.FirstOrDefaultAsync(p => p.Email.Address.ToLower() == email.ToLower());

        public async Task<Patient?> GetPatientByIdAsync(Guid id) 
            => await _entity.FindAsync(id);
    }
}
