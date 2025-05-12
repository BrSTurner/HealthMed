using Med.Application.Models;
using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
using Med.SharedKernel.Models;
using Med.SharedKernel.UoW;
using System.Linq;

namespace Med.Application.Services
{
    public class CalendarService(IMessageBus bus,
        ICalendarRepository calendarRepository,
        IBookingTimeRepository bookingTimeRepository,
        IUnitOfWork unitOfWork) : ICalendarService
    {
        private readonly IMessageBus _bus = bus;
        private readonly ICalendarRepository _calendarRepository = calendarRepository;
        private readonly IBookingTimeRepository _bookingTimeRepository = bookingTimeRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<DomainResult> CreateDoctorCalendar(CreateDoctorCalendarInput input)
        {
            var calendarEntity = new Calendar()
            {
                DoctorId = input.DoctorId,
                Price = input.Price,
                Bookings = new List<BookingTime>()
            };

            var alreadyExistedCalendar = await _calendarRepository.GetCalendarByDoctorId(calendarEntity.DoctorId);
            if(alreadyExistedCalendar != null)
            {
                return DomainResult.Error("Já tem um calendario para este médico!");
            }

            _calendarRepository.CreateDoctorCalendar(calendarEntity);
            if (!await _unitOfWork.SaveChanges())
                return DomainResult.Error("Não foi possível criar o calendário!");

            var bookingEntities = input.BookingTime.Select(x => MapBookingTime(x, calendarEntity)).ToList();
            await _bookingTimeRepository.CreateCalendarBookingTime(bookingEntities);

            var dto = MapCalendarDto(calendarEntity);

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success(dto);

            return DomainResult.Error("Nao foi possivel criar a calendario!");
        }

        public async Task<CalendarDTO?> GetCalendarById(Guid calendarId)
        {
            var entity = await _calendarRepository.GetCalendarById(calendarId);
            if (entity == null)
            {
                return null;
            }

            var dto = MapCalendarDto(entity);

            return dto;
        }

        public async Task<CalendarDTO?> GetCalendarByDoctorId(Guid doctorId)
        {
            var entity = await _calendarRepository.GetCalendarByDoctorId(doctorId);
            if (entity == null)
            {
                return null;
            }

            var dto =  MapCalendarDto(entity);

            return dto;
        }

        public async Task<UpdateCalendarAppointmentResponse> UpdateCalendarAppointment(UpdateCalendarAppointmentRequest request)
        {
            var booking = await _bookingTimeRepository.GetBookingTimeById(request.BookingTimeId);
            if (booking == null)
            {
                return new UpdateCalendarAppointmentResponse
                {
                    Success = false,
                    ErrorMessage = "Horário selecionado não foi possível de ser achado"
                };
            }

            if (request.IsCancelled.HasValue && request.IsCancelled.Value)
            {
                booking.Status = BookingTimeStatus.Available;
            }
            else
            {
                if (booking.Status == BookingTimeStatus.Unavailable)
                {
                    return new UpdateCalendarAppointmentResponse
                    {
                        Success = false,
                        ErrorMessage = "Horário selecionado não está disponível"
                    };
                }

                booking.Status = BookingTimeStatus.Unavailable;
            }


            if (await _unitOfWork.SaveChanges())
                return new UpdateCalendarAppointmentResponse { Success = true };

            return new UpdateCalendarAppointmentResponse { Success = false };
        }

        public async Task<DomainResult> UpdateDoctorCalendar(UpdateDoctorCalendarInput input)
        {
            var calendar = await _calendarRepository.GetCalendarById(input.Id);
            if (calendar == null)
            {
                return DomainResult.Error("Nao foi possivel achar o calendario do Doutor especificado!");
            }

            if(input.Price.HasValue)
            {
                calendar.Price = input.Price.Value;
            }

            var newBookingsTime = input.Bookings.Where(x => !x.Id.HasValue).Select(x => MapBookingTime(x, calendar)).ToList();  
            if(newBookingsTime.Count() > 0)
            {
                await _bookingTimeRepository.CreateCalendarBookingTime(newBookingsTime);
            }

            var oldBookingTime = input.Bookings.Where(x => x.Id.HasValue).Select(x => MapBookingTime(x, calendar)).ToList();

            if (oldBookingTime.Count() > 0)
            {
                _bookingTimeRepository.UpdateCalendarBookingTime(oldBookingTime);
            }

            //input.Bookings.Where(x => x.Id.HasValue).Select(x => MapBookingTime(x, calendar)).ToList().ForEach(booking =>
            //{
            //    var entity = calendar.Bookings.FirstOrDefault(x => x.Id == booking.Id);
            //    if (entity != null)
            //    {
            //        entity.Date = booking.Date;
            //    }
            //});



            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success();

            return DomainResult.Error("Nao foi possivel criar a calendario!");
        }

        private BookingTime MapBookingTime(BookingTimeInput input, Calendar entity)
        {
            return new BookingTime
            {
                Id = input.Id.HasValue ? input.Id.Value : Guid.NewGuid(),
                CalendarId = entity.Id,
                CreatedAt = DateTime.Now,
                Status = BookingTimeStatus.Available,
                Date = input.Date,
                Calendar = entity
            };
        }

        private BookingTimeDto MapBookingTimeDto(BookingTime entity)
        {
            return new BookingTimeDto
            {
                Id = entity.Id,
                Date = entity.Date,
                Status = entity.Status,
                ConsultDuration = entity.ConsultDuration,   
            };
        }

        private CalendarDTO MapCalendarDto(Calendar entity)
        {
            return new CalendarDTO
            {
                Id = entity.Id,
                Bookings = entity.Bookings.Select(x => MapBookingTimeDto(x)).ToList() ?? new List<BookingTimeDto>(),
                Price = entity.Price,
            };
        }
    }
}
