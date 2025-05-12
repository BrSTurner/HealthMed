using MassTransit;
using Med.MessageBus.Integration.Requests.Appointments;

namespace Med.application.Consumers
{
    public class AppointmentConsumer() : IConsumer<GetDoctorByAppointmentRequest>
    {

        public Task Consume(ConsumeContext<GetDoctorByAppointmentRequest> context)
        {
            return Task.CompletedTask;
        }

    }
}
