using FluentValidation;
using Med.Application.Models.Inputs;
using Med.SharedKernel.Enumerations;

namespace Med.Application.Validations
{
    public class CreateUserInputValidation : AbstractValidator<CreateUserInput>
    {
        public CreateUserInputValidation()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Tipo do usuário é obrigatório");

            When(x => x.Type == UserType.Doctor, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Nome do médico é obrigatório.");

                RuleFor(x => x.CRM)
                    .NotEmpty()
                    .WithMessage("CRM do médico é obrigatório.");

                RuleFor(x => x.SpecialityId)
                    .NotEmpty()
                    .WithMessage("Especialidade do médico é obrigatória.");
            });

            When(x => x.Type == UserType.Patient, () =>
            {
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage("Nome do paciente é obrigatório.");

                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("O Email é obrigatório.")
                    .EmailAddress().WithMessage("O Email informado não é válido.");

                RuleFor(x => x.CPF)
                    .NotEmpty().WithMessage("O CPF é obrigatório.");

            });

            RuleFor(x => x.Password)
                    .NotEmpty()
                    .WithMessage("Senha é obrigatória");
        }
    }
}
