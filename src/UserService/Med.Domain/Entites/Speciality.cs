using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entites
{
    public class Speciality : Entity, IAggregateRoot
    {
        public required string Name { get; set; }
        public virtual ICollection<Doctor>? Doctors { get; set; }
    }
}
