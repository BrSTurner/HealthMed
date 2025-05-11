using FluentValidation.Results;
using Med.Application.Validations;
using Med.SharedKernel.Models;

namespace Med.Application.Models
{
    public class CancelAppointmentInput : Input
    {
        public required Guid AppointmentId { get; set; }

        public required Guid BookingTimeId { get; set; }

        public string? ReasonForCanceling { get; set; }

        public override ValidationResult Validate()
            => new CancelAppointmentInputValidation().Validate(this);

    }
}
