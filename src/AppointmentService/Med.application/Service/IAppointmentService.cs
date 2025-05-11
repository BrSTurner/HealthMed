using Med.Application.Models;
using Med.SharedKernel.Models;

namespace Med.Application.Services
{
    public interface IAppointmentService
    {
        public Task<List<AppointmentDTO>> GetAppointmentsByDoctor(Guid doctorId);

        public Task<DomainResult> CreateAppointment(CreateAppointmentInput createAppointmentInput);

        Task<DomainResult> CancelAppointment(CancelAppointmentInput createAppointmentInput);

        Task<DomainResult> ReplyAppointment(ReplyAppointmentInput replyAppointmentInput);
    }
}
