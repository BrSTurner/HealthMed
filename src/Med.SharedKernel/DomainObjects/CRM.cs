using System.Text.RegularExpressions;

namespace Med.SharedKernel.DomainObjects
{
    public partial class CRM
    {
        public string Number { get; set; }

        protected CRM()
        {
            Number = string.Empty;
        }

        public CRM(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CRM não pode ser vazio.");

            if (!CRMRegex().IsMatch(value))
                throw new ArgumentException("CRM inválido. Formato esperado: 123456/SP");

            Number = value;
        }

        public static CRM Create(string number) 
            => new (number);        

        public override string ToString() => Number;

        public override bool Equals(object? obj) => obj is CRM other && Number == other.Number;

        public override int GetHashCode() => Number.GetHashCode();

        [GeneratedRegex(@"^\d{4,6}\/[A-Z]{2}$")]
        private static partial Regex CRMRegex();
    }
}
