using FluentValidation.Results;
using Med.Application.Validations;
using Med.SharedKernel.Enumerations;
using Med.SharedKernel.Models;

namespace Med.Application.Models.Inputs
{
    public class CreateUserInput : Input
    {
        public required UserType Type { get; set; }
        public required string Name { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
        public string? CPF {  get; set; }
        public string? CRM { get; set; }
        public Guid? SpecialityId { get; set; }

        public override ValidationResult Validate()
            => new CreateUserInputValidation().Validate(this);        
    }
}
