using Med.Application.Models;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
using Med.SharedKernel.Models;

namespace Med.Application.Services
{
    public interface ICalendarService
    {
        Task<CalendarDTO?> GetCalendarById(Guid calendarId);

        Task<CalendarDTO?> GetCalendarByDoctorId(Guid doctorId);

        Task<DomainResult> CreateDoctorCalendar(CreateDoctorCalendarInput createDoctorCalendarInput);

        Task<DomainResult> UpdateDoctorCalendar(UpdateDoctorCalendarInput updateDoctorCalendarInput);

        Task<UpdateCalendarAppointmentResponse> UpdateCalendarAppointment(UpdateCalendarAppointmentRequest request);
    }
}
