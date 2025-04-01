using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;

namespace Med.Infrastructure.Repositories
{
    public class PatientRepository(UserContext context) : BaseRepository<Patient>(context), IPatientRepository
    {
    }
}
