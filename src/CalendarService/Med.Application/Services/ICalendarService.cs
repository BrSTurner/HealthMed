using Med.Application.Models;
using Med.SharedKernel.Models;

namespace Med.Application.Services
{
    public interface ICalendarService
    {
        Task<CalendarDTO> GetAvailableCalendarsByDoctor(Guid doctorId);

        Task<DomainResult> CreateDoctorCalendar(CreateDoctorCalendarInput createDoctorCalendarInput);
    }
}
