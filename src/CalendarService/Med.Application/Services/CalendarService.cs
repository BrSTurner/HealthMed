using Med.Application.Models;
using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
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
            };

            var bookingEntities = input.BookingTime.Select(x => MapBookingTime(x, calendarEntity));  

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

        public async Task<UpdateCalendarIfAvailableResponse> UpdateCalendarIfAvailable(UpdateCalendarIfAvailableRequest request)
        {
            var doctorCalendar = await _calendarRepository.GetCalendarByDoctor(request.DoctorId);

            if (doctorCalendar == null)
            {
                return new UpdateCalendarIfAvailableResponse
                {
                    Success = false,
                    ErrorMessage = "Não foi possível encontrar o Doutor"
                };
            }

            var booking = doctorCalendar?.Bookings.SingleOrDefault(x => x.Date == request.Date);

            if (booking == null)
            {
                return new UpdateCalendarIfAvailableResponse
                {
                    Success = false,
                    ErrorMessage = "Horário selecionado não foi possível de ser achado"
                };
            }

            if (booking.Status == BookingTimeStatus.Unavailable)
            {
                return new UpdateCalendarIfAvailableResponse
                {
                    Success = false,
                    ErrorMessage = "Horário selecionado não está disponível"
                };
            }

            booking.Status = BookingTimeStatus.Unavailable;

            if (await _unitOfWork.SaveChanges())
                return new UpdateCalendarIfAvailableResponse { Success = true };

            return new UpdateCalendarIfAvailableResponse { Success = false };
        }

        public async Task<DomainResult> UpdateDoctorCalendar(UpdateDoctorCalendarInput input)
        {
            var calendar = await _calendarRepository.GetCalendarByDoctor(input.DoctorId);
            if (calendar == null)
            {
                return DomainResult.Error("Nao foi possivel achar o calendario do Doutor especificado!");
            }

            if(input.Price.HasValue)
            {
                calendar.Price = input.Price.Value;
            }

            if(input.Bookings.Count > 0)
            {
                calendar.Bookings = input.Bookings;
            }

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success();

            return DomainResult.Error("Nao foi possivel criar a calendario!");
        }

        private BookingTime MapBookingTime(BookingTimeInput input, Calendar entity)
        {
            return new BookingTime
            {
                CalendarId = input.CalendarId,
                ConsultDuration = input.ConsultDuration,
                CreatedAt = DateTime.Now,
                Status = BookingTimeStatus.Available,
                Date = input.Date,
                Calendar = entity
            };
        }
    }
}
