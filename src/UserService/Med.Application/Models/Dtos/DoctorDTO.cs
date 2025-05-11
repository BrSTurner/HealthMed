using Med.SharedKernel.DomainObjects;

namespace Med.Application.Models.Dtos
{
    public class DoctorDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required CRM CRM { get; set; }
        public required Guid SpecialityId { get; set; }
    }
}
