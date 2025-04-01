using System.ComponentModel.DataAnnotations;

namespace Med.SharedKernel.DomainObjects
{
    public sealed class Email
    {
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        public string Address { get; init; }

        public Email(string? address)
        {
            if (!IsValidEmail(address))
            {
                throw new ArgumentException("Formato de e-mail inválido.", nameof(address));
            }

            Address = address ?? string.Empty;
        }

        public static Email Create(string? email) 
            => new (email);

        private static bool IsValidEmail(string? email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        public override string ToString() => Address;

        public override bool Equals(object? obj)
        {
            if (obj is Email other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(Email other)
        {
            if (other is null)
            {
                return false;
            }

            return Address == other.Address;
        }

        public override int GetHashCode()
            => Address != null ? Address.GetHashCode() : 0;
        
        public static bool operator ==(Email left, Email right)
            => Equals(left, right);
        
        public static bool operator !=(Email left, Email right)
            => !Equals(left, right);
        
    }
}
