using Med.Application.Models.Dtos;
using Med.Domain.Entites;

namespace Med.Application.Mappers
{
    public static class UserMapper
    {
        public static DoctorDTO MapDoctorDTO(Doctor entity)
        {
            return new DoctorDTO
            {
                Id = entity.Id,
                CRM = entity.CRM,
                SpecialityId = entity.SpecialityId,
                Name = entity.Name,
            };
        }

        public static PatientDTO MapPatientDTO(Patient entity)
        {
            return new PatientDTO
            {
                Id = entity.Id,
                CPF = entity.CPF,
                Email = entity.Email,
                Name = entity.Name,
            };
        }
    }
}
