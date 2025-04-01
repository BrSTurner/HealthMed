using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entites
{
    public class Doctor : User, IAggregateRoot
    {
        public required CRM CRM { get; set; }
        public Speciality? Speciality { get; set; }
        public required Guid SpecialityId { get; set; }
    }
}
