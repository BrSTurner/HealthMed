using Med.Application.Models;
using Med.Application.Services;
using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.SharedKernel.UoW;
using NSubstitute;
using Shouldly;

namespace Med.CalendarTests
{
    public class CalendarServiceTests
    {
        private readonly ICalendarRepository _calendarRepository = Substitute.For<ICalendarRepository>();
        private readonly IBookingTimeRepository _bookingTimeRepository = Substitute.For<IBookingTimeRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

        private readonly CalendarService _service;

        public CalendarServiceTests()
        {
            _service = new CalendarService(_calendarRepository, _bookingTimeRepository, _unitOfWork);
        }

        [Fact(DisplayName = "Create Calendar")]
        [Trait("Calendar", "Create")]
        public async Task CreateDoctorCalendar_Should_Create_Calendar_When_Valid()
        {
            // Arrange
            var input = new CreateDoctorCalendarInput
            {
                DoctorId = Guid.NewGuid(),
                Price = 100,
                BookingTime = [new() { Date = DateTime.Today }]
            };

            var calendarId = Guid.NewGuid();

            var calendar = new Calendar
            {
                Id = calendarId,
                DoctorId = input.DoctorId,
                Price = input.Price,
            };

            var bookings = new List<BookingTime> { new() { CalendarId = calendarId, Date = new DateTime(), Id = Guid.NewGuid(), Status = BookingTimeStatus.Available, Calendar = calendar } };

            calendar.Bookings = bookings;

            _calendarRepository.GetCalendarByDoctorId(input.DoctorId)
                .Returns(Task.FromResult<Calendar?>(null));



            _unitOfWork.SaveChanges().Returns(true);

            // Act
            var result = await _service.CreateDoctorCalendar(input);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            await _calendarRepository.Received(1).CreateDoctorCalendar(Arg.Any<Calendar>());
        }



        [Fact(DisplayName = "Get Calendar")]
        [Trait("Calendar", "Get")]
        public async Task GetCalendarById_WhenExists_ReturnsDto()
        {
            // Arrange
            var calendarId = Guid.NewGuid();
            var calendar = new Calendar 
            { 
                DoctorId = Guid.NewGuid(),
                Id = calendarId, 
                Price = 200, 
                Bookings = [] 
            };
            _calendarRepository.GetCalendarById(calendarId).Returns(calendar);

            // Act
            var result = await _service.GetCalendarById(calendarId);

            // Assert
            result.ShouldNotBeNull();
            result!.Id.ShouldBe(calendarId);
            result.Price.ShouldBe(200);
        }

        [Fact(DisplayName = "Get Non Existent Calendar")]
        [Trait("Calendar", "Get")]
        public async Task GetCalendarById_WhenNotExists_ReturnsNull()
        {
            // Arrange
            var calendarId = Guid.NewGuid();
            _calendarRepository.GetCalendarById(calendarId).Returns((Calendar?)null);

            // Act
            var result = await _service.GetCalendarById(calendarId);

            // Assert
            result.ShouldBeNull();
        }

        [Fact(DisplayName = "Update Calendar Appointment")]
        [Trait("Calendar", "Update")]
        public async Task UpdateCalendarAppointment_WhenBookingNotFound_ReturnsError()
        {
            // Arrange
            var request = new UpdateCalendarAppointmentRequest { BookingTimeId = Guid.NewGuid() };
            _bookingTimeRepository.GetBookingTimeById(request.BookingTimeId).Returns((BookingTime?)null);

            // Act
            var result = await _service.UpdateCalendarAppointment(request);

            // Assert
            result.Success.ShouldBeFalse();
            result.ErrorMessage?.ShouldContain("Horário selecionado não foi encontrado");
        }

        [Fact(DisplayName = "Update Doctor Calendar")]
        [Trait("Calendar", "Update")]
        public async Task UpdateDoctorCalendar_WhenCalendarNotFound_ReturnsError()
        {
            // Arrange
            var input = new UpdateDoctorCalendarInput { Id = Guid.NewGuid(), Bookings = [], Price = 150 };
            _calendarRepository.GetCalendarById(input.Id).Returns((Calendar?)null);

            // Act
            var result = await _service.UpdateDoctorCalendar(input);

            // Assert
            result.Errors?.ShouldContain("Nao foi possivel achar o calendario do Doutor especificado!");
        }

        [Fact(DisplayName = "Update Valid Doctor Calendar")]
        [Trait("Calendar", "Update")]
        public async Task UpdateDoctorCalendar_WhenValid_UpdatesSuccessfully()
        {
            // Arrange
            var calendar = new Calendar 
            { 
                Id = Guid.NewGuid(), 
                DoctorId = Guid.NewGuid(), 
                Bookings = [] 
            };

            var input = new UpdateDoctorCalendarInput
            {
                Id = calendar.Id,
                Price = 300,
                Bookings =
                [
                    new() { Date = DateTime.Today }
                ]
            };

            _calendarRepository.GetCalendarById(calendar.Id).Returns(calendar);
            _unitOfWork.SaveChanges().Returns(true);

            // Act
            var result = await _service.UpdateDoctorCalendar(input);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            calendar.Price.ShouldBe(300);
            calendar.Bookings.Count.ShouldBe(0);
        }


    }
}
