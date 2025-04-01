using System.Text.RegularExpressions;

namespace Med.SharedKernel.DomainObjects
{
    public sealed partial class CRM
    {
        private static readonly Regex CRMValidator = RegexValidator();

        public string Number { get; }

        private CRM(string value)
        {
            Number = value;
        }

        public static CRM Create(string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("CRM não pode ser vazio.");

            if (!CRMValidator.IsMatch(number))
                throw new ArgumentException("CRM inválido. Formato esperado: 123456/SP");

            return new CRM(number);
        }

        public override string ToString() => Number;

        public override bool Equals(object? obj) => obj is CRM other && Number == other.Number;

        public override int GetHashCode() => Number.GetHashCode();

        [GeneratedRegex(@"^\d{4,6}\/[A-Z]{2}$", RegexOptions.Compiled)]
        private static partial Regex RegexValidator();
    }
}
