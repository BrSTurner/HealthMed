using Med.Application.Interfaces.Services;
using Med.Application.Models.Dtos;
using Med.Domain.Repositories;

namespace Med.Application.Services
{
    public class SpecialityService(ISpecialityRepository specialityRepository) : ISpecialityService
    {
        private readonly ISpecialityRepository _specialityRepository = specialityRepository;

        public async Task<List<DoctorDTO>> GetDoctorsBySpeciality(Guid specialityId)
        {
            var entity = await _specialityRepository.GetDoctorsBySpeciality(specialityId);
            if (entity == null)
            {
                return [];
            }

            return entity.Doctors.Select(UserService.MapDoctorDTO).ToList();
        }
    }
}
