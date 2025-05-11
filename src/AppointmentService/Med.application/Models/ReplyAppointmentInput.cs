using FluentValidation.Results;
using Med.Application.Validations;
using Med.Domain.Enumerations;
using Med.SharedKernel.Models;

namespace Med.Application.Models
{
    public class ReplyAppointmentInput : Input
    {
        public required Guid AppointmentId { get; set; }
        public required Guid BookingTimeId { get; set; }
        public required AppointmentStatus Status { get; set; }

        public override ValidationResult Validate()
            => new ReplyAppointmentInputValidation().Validate(this);
    }
}
