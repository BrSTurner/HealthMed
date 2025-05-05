using System.Text.RegularExpressions;

namespace Med.SharedKernel.DomainObjects
{
    public partial class CPF
    {
        public string Number { get; set; }
        
        protected CPF() 
        {
            Number = string.Empty;
        }

        public CPF(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("CPF não pode ser vazio.");

            if (!CPFRegex().IsMatch(value) || !IsValidCpf(value))
                throw new ArgumentException("CPF inválido.");

            Number = value;
        }

        public static CPF Create(string number) 
            => new (number);        

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

        [GeneratedRegex(@"^\d{3}\.\d{3}\.\d{3}\-\d{2}$")]
        private static partial Regex CPFRegex();
    }
}
