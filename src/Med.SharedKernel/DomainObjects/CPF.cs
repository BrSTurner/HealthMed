using System.Text.RegularExpressions;

namespace Med.SharedKernel.DomainObjects
{
    public sealed partial class CPF
    {
        private static readonly Regex CPFValidator = RegexValidator();

        public string Number { get; }

        private CPF(string value)
        {
            Number = value;
        }

        public static CPF Create(string? number)
        {
            if (string.IsNullOrWhiteSpace(number))
                throw new ArgumentException("CPF não pode ser vazio.");

            if (!CPFValidator.IsMatch(number) || !IsValidCpf(number))
                throw new ArgumentException("CPF inválido.");

            return new CPF(number);
        }

        private static bool IsValidCpf(string cpf)
        {
            var numbers = cpf.Replace(".", "").Replace("-", "").Select(c => c - '0').ToArray();

            if (numbers.Length != 11 || numbers.Distinct().Count() == 1)
                return false;

            for (int j = 0; j < 2; j++)
            {
                int sum = 0;
                for (int i = 0; i < 9 + j; i++)
                    sum += numbers[i] * (10 + j - i);

                int remainder = sum % 11;

                if ((remainder < 2 ? 0 : 11 - remainder) != numbers[9 + j])
                    return false;
            }

            return true;
        }

        public override string ToString() => Number;

        public override bool Equals(object? obj) => obj is CPF other && Number == other.Number;

        public override int GetHashCode() => Number.GetHashCode();

        [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$", RegexOptions.Compiled)]
        private static partial Regex RegexValidator();
    }
}
