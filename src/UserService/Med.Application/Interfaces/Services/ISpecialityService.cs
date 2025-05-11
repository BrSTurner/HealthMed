using Med.Application.Models.Dtos;

namespace Med.Application.Interfaces.Services
{
    public interface ISpecialityService
    {
        Task<List<DoctorDTO>> GetDoctorsBySpeciality(Guid specialityId);
    }
}
