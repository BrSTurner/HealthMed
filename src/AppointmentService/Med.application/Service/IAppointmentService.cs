using Med.Application.Models;
using Med.SharedKernel.Models;

namespace Med.Application.Services
{
    public interface IAppointmentService
    {
        //public Task<List<AppointmentDTO>> GetAppointmentsByDoctor(Guid doctorId);

        public Task<DomainResult> CreateAppointment(CreateAppointmentInput createAppointmentInput);

        public void CancelAppointment(CancelAppointmentInput createAppointmentInput);

        public void ReplyAppointment(ReplyAppointmentInput replyAppointmentInput);
    }
}
