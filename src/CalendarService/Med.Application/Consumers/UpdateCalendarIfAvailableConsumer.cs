using MassTransit;
using Med.Application.Services;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;

namespace Med.Application.Consumers
{
    public class UpdateCalendarIfAvailableConsumer(ICalendarService calendarService) : IConsumer<UpdateCalendarAppointmentRequest>
    {
        private readonly ICalendarService _calendarService = calendarService;

        public async Task Consume(ConsumeContext<UpdateCalendarAppointmentRequest> context)
        {
            var response = new UpdateCalendarAppointmentResponse();

            try
            {
                response = await _calendarService.UpdateCalendarAppointment(context.Message);
            }
            catch (Exception e)
            {
                response = new() { ErrorMessage = $"Algo deu errado ao criar o usuário, {e.Message}" };
            }
            finally
            {
                await context.RespondAsync(response);
            }

        }
    }
}
