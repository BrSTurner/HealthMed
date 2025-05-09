using Med.Application.Models;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
using Med.SharedKernel.Models;

namespace Med.Application.Services
{
    public interface ICalendarService
    {
        Task<CalendarDTO?> GetAvailableCalendarsByDoctor(Guid doctorId);

        Task<DomainResult> CreateDoctorCalendar(CreateDoctorCalendarInput createDoctorCalendarInput);

        Task<DomainResult> UpdateDoctorCalendar(UpdateDoctorCalendarInput updateDoctorCalendarInput);

        Task<UpdateCalendarIfAvailableResponse> UpdateCalendarIfAvailable(UpdateCalendarIfAvailableRequest request);
    }
}
