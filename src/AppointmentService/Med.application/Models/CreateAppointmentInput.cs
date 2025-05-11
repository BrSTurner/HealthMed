using FluentValidation.Results;
using Med.Application.Validations;
using Med.SharedKernel.Models;

namespace Med.Application.Models
{
    public class CreateAppointmentInput : Input
    {
        public required Guid DoctorId { get; set; }
        public required Guid CalendarId { get; set; }
        public required Guid BookingTimeId { get; set; }
        public required Guid PatientId { get; set; }
        public required DateTime Date { get; set; }

        public override ValidationResult Validate()
            => new CreateAppointmentInputValidation().Validate(this);
    }
}
