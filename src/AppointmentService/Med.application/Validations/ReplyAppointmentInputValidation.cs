using FluentValidation;
using Med.Application.Models;

namespace Med.Application.Validations
{
    public class ReplyAppointmentInputValidation : AbstractValidator<ReplyAppointmentInput>
    {
        public ReplyAppointmentInputValidation()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty()
                .WithMessage("Agendamento não foi encontrado.");
        }
    }
}
