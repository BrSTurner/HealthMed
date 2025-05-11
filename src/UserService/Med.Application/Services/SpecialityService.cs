using Med.Application.Interfaces.Services;
using Med.Application.Mappers;
using Med.Application.Models.Dtos;
using Med.Domain.Repositories;

namespace Med.Application.Services
{
    public class SpecialityService(ISpecialityRepository specialityRepository) : ISpecialityService
    {
        private readonly ISpecialityRepository _specialityRepository = specialityRepository;

        public async Task<List<DoctorDTO>> GetDoctorsBySpeciality(Guid specialityId)
        {
            var speciality = await _specialityRepository.GetDoctorsBySpeciality(specialityId);
            
            if (speciality == null) return [];

            return speciality.Doctors?.Select(UserMapper.MapDoctorDTO)?.ToList() ?? [];
        }
    }
}
