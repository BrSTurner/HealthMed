using FluentValidation;
using Med.Domain.Entities;
using Med.SharedKernel.Enumerations;

namespace Med.Domain.Validations
{
    public class UserValidation : AbstractValidator<User>
    {
        public UserValidation()
        {
            RuleFor(x => x.Type)
                .NotEmpty()
                .WithMessage("Tipo do usuário é obrigatório");

            When(x => x.Type == UserType.Doctor, () =>
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .WithMessage("Nome do médico é obrigatório.");
            });

            When(x => x.Type == UserType.Patient, () =>
            {
                RuleFor(x => x.Username)
                    .NotEmpty()
                    .WithMessage("Nome do paciente é obrigatório.");

                RuleFor(x => x.Email)
                    .NotNull()
                    .WithMessage("O Email é obrigatório.");

                RuleFor(x => x.Email.Address)
                    .NotEmpty().WithMessage("O Email é obrigatório.")
                    .EmailAddress().WithMessage("O Email informado não é válido.");                    
            });

            RuleFor(x => x.PasswordHash)
                    .NotEmpty()
                    .WithMessage("Senha é obrigatória");
        }
    }
}
