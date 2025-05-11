using FluentValidation;
using Med.Application.Models;

namespace Med.Application.Validations
{
    public class CancelAppointmentInputValidation : AbstractValidator<CancelAppointmentInput>
    {
        public CancelAppointmentInputValidation()
        {
            RuleFor(x => x.AppointmentId)
                .NotEmpty()
                .WithMessage("Agendamento não foi encontrado.");

            RuleFor(x => x.BookingTimeId)
                .NotEmpty()
                .WithMessage("Horario não foi encontrado.");
        }
    }
}
