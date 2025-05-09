using MassTransit;
using Med.Application.Services;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;

namespace Med.Application.Consumers
{
    public class UpdateCalendarIfAvailableConsumer(ICalendarService calendarService) : IConsumer<UpdateCalendarIfAvailableRequest>
    {
        private readonly ICalendarService _calendarService = calendarService;

        public async Task Consume(ConsumeContext<UpdateCalendarIfAvailableRequest> context)
        {
            var response = new UpdateCalendarIfAvailableResponse();

            try
            {
                response = await _calendarService.UpdateCalendarIfAvailable(context.Message);
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
