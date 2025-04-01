using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.Infrastructure.Data;

namespace Med.Infrastructure.Repositories
{
    public class SpecialityRepository(UserContext context) : BaseRepository<Speciality>(context), ISpecialityRepository
    {
    }
}
