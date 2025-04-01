using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;

namespace Med.Infrastructure.Repositories
{
    public class DoctorRepository(UserContext context) : BaseRepository<Doctor>(context), IDoctorRepository
    {
    }
}
