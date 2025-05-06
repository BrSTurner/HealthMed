using Med.Application.Models;
using Med.Domain.Entities;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.SharedKernel.Models;
using Med.SharedKernel.UoW;

namespace Med.Application.Services
{
    public class CalendarService(IMessageBus bus,
        ICalendarRepository calendarRepository,
        IUnitOfWork unitOfWork) : ICalendarService
    {
        private readonly IMessageBus _bus = bus;
        private readonly ICalendarRepository _calendarRepository = calendarRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<DomainResult> CreateDoctorCalendar(CreateDoctorCalendarInput input)
        {
            var calendarEntity = new Calendar()
            {
                DoctorId = input.DoctorId,
                Price = input.Price,
                Bookings = input.BookingTime
            };

            _calendarRepository.CreateDoctorCalendar(calendarEntity);

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success();

            return DomainResult.Error("Nao foi possivel criar a calendario!");
        }

        public async Task<CalendarDTO> GetAvailableCalendarsByDoctor(Guid doctorId)
        {
            var entity = await _calendarRepository.GetCalendarByDoctor(doctorId);

            var dto = new CalendarDTO
            {
                Bookings = entity.Bookings ?? [],
                Price = entity.Price
            };

            return dto;
    
        }
    }
}
