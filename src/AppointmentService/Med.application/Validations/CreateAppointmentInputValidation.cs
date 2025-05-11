using FluentValidation;
using Med.Application.Models;

namespace Med.Application.Validations
{
    public class CreateAppointmentInputValidation : AbstractValidator<CreateAppointmentInput>
    {
        public CreateAppointmentInputValidation()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage("CRM do médico é obrigatório.");

            RuleFor(x => x.Date)
                .NotEmpty()
                .WithMessage("Data do agendamento é obrigatória.");

            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("Paciente é obrigatório.");

            RuleFor(x => x.CalendarId)
                .NotEmpty()
                .WithMessage("Calendario do médico é obrigatório.");

            RuleFor(x => x.BookingTimeId)
                .NotEmpty()
                .WithMessage("Horario do agendamento é obrigatório.");

            RuleFor(x => x.Date)
                .Custom((date, context) =>
                {
                    var today = DateTime.Today;
                    var maxDate = today.AddMonths(6);

                    if (date < today)
                    {
                        context.AddFailure("A data não pode ser no passado.");
                    }
                    else if (date > maxDate)
                    {
                        context.AddFailure("A data deve estar dentro dos próximos 6 meses.");
                    }
                });

        }
    }
}
