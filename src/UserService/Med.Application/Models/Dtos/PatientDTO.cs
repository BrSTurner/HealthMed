using Med.SharedKernel.DomainObjects;

namespace Med.Application.Models.Dtos
{
    public class PatientDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required CPF CPF { get; set; }
        public required Email Email { get; set; }
    }
}
