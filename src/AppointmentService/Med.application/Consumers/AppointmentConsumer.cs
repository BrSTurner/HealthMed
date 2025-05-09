using MassTransit;
using Med.Application.Services;
using Med.MessageBus.Integration.Requests.Appointments;
using Med.MessageBus.Integration.Responses.Users;

namespace Med.application.Consumers
{
    public class AppointmentConsumer(IAppointmentService appointmentService) : IConsumer<GetDoctorByAppointmentRequest>
    {
        private readonly IAppointmentService _appointmentService = appointmentService;

        public async Task Consume(ConsumeContext<GetDoctorByAppointmentRequest> context)
        {

        }

    }
}
