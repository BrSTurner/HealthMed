using Med.SharedKernel.DomainObjects;

namespace Med.Domain.Entites
{
    public class Patient : User, IAggregateRoot
    {
        public required CPF CPF { get; set; }       
        public required Email Email { get; set; }
    }
}
