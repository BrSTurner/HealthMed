using Med.Application.Models;
using Med.Application.Services;
using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Appointments;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
using Med.SharedKernel.UoW;
using NSubstitute;
using Shouldly;

namespace Med.AppointmentTests
{
    public class AppointmentServiceTests
    {
        private readonly IMessageBus _bus;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppointmentService _service;

        public AppointmentServiceTests()
        {
            _bus = Substitute.For<IMessageBus>();
            _appointmentRepository = Substitute.For<IAppointmentRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _service = new AppointmentService(_bus, _appointmentRepository, _unitOfWork);
        }

        [Fact]
        public async Task CreateAppointment_Should_Create_When_Valid()
        {
            // Arrange
            var input = new CreateAppointmentInput
            {
                CalendarId = Guid.NewGuid(),
                DoctorId = Guid.NewGuid(),
                PatientId = Guid.NewGuid(),
                BookingTimeId = Guid.NewGuid(),
                Date = DateTime.Today
            };

            var doctorResponse = new GetDoctorByAppointmentResponse
            {
                Success = true,
                DoctorId = input.DoctorId
            };

            var calendarResponse = new UpdateCalendarAppointmentResponse
            {
                Success = true
            };

            _bus.RequestAsync<GetDoctorByAppointmentRequest, GetDoctorByAppointmentResponse>(Arg.Any<GetDoctorByAppointmentRequest>())
                .Returns(doctorResponse);

            _bus.RequestAsync<UpdateCalendarAppointmentRequest, UpdateCalendarAppointmentResponse>(Arg.Any<UpdateCalendarAppointmentRequest>())
                .Returns(calendarResponse);

            _unitOfWork.SaveChanges().Returns(true);

            // Act
            var result = await _service.CreateAppointment(input);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            _appointmentRepository.Received(1).AddAppointmentAsync(Arg.Any<Appointment>());
            await _unitOfWork.Received(1).SaveChanges();
        }


        [Fact]
        public async Task CancelAppointment_Should_Cancel_When_Valid()
        {
            // Arrange
            var input = new CancelAppointmentInput
            {
                AppointmentId = Guid.NewGuid(),
                BookingTimeId = Guid.NewGuid(),
                ReasonForCanceling = "Surgiu um compromisso"
            };

            var appointment = new Appointment
            {
                Id = input.AppointmentId,
                DoctorId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                PatientId = Guid.NewGuid(),
                Status = AppointmentStatus.Pending
            };

            var calendarResponse = new UpdateCalendarAppointmentResponse
            {
                Success = true
            };

            _bus.RequestAsync<UpdateCalendarAppointmentRequest, UpdateCalendarAppointmentResponse>(Arg.Any<UpdateCalendarAppointmentRequest>())
                .Returns(calendarResponse);

            _appointmentRepository.GetAppointmentByIdAsync(input.AppointmentId).Returns(Task.FromResult<Appointment?>(appointment));
            _unitOfWork.SaveChanges().Returns(true);

            // Act
            var result = await _service.CancelAppointment(input);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            appointment.Status.ShouldBe(AppointmentStatus.Canceled);
            appointment.ReasonForCanceling.ShouldBe(input.ReasonForCanceling);
        }

        [Fact]
        public async Task ReplyAppointment_Should_Reply_When_Valid_And_Approved()
        {
            // Arrange
            var input = new ReplyAppointmentInput
            {
                AppointmentId = Guid.NewGuid(),
                BookingTimeId = Guid.NewGuid(),
                Status = AppointmentStatus.Confirmed
            };

            var appointment = new Appointment
            {
                Id = input.AppointmentId,
                DoctorId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                PatientId = Guid.NewGuid(),
                Status = AppointmentStatus.Pending,
            };

            _appointmentRepository.GetAppointmentByIdAsync(input.AppointmentId)
                .Returns(Task.FromResult<Appointment?>(appointment));

            _unitOfWork.SaveChanges().Returns(true);

            // Act
            var result = await _service.ReplyAppointment(input);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            appointment.Status.ShouldBe(AppointmentStatus.Confirmed);
        }

        [Fact]
        public async Task ReplyAppointment_Should_Reply_When_Refused_And_Update_Calendar()
        {
            // Arrange
            var input = new ReplyAppointmentInput
            {
                AppointmentId = Guid.NewGuid(),
                BookingTimeId = Guid.NewGuid(),
                Status = AppointmentStatus.Refused
            };

            var appointment = new Appointment
            {
                Id = input.AppointmentId,
                DoctorId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                PatientId = Guid.NewGuid(),
                Status = AppointmentStatus.Pending
            };

            var calendarResponse = new UpdateCalendarAppointmentResponse
            {
                Success = true
            };

            _bus.RequestAsync<UpdateCalendarAppointmentRequest, UpdateCalendarAppointmentResponse>(Arg.Any<UpdateCalendarAppointmentRequest>())
                .Returns(calendarResponse);

            _appointmentRepository.GetAppointmentByIdAsync(input.AppointmentId)
                .Returns(Task.FromResult<Appointment?>(appointment));

            _unitOfWork.SaveChanges().Returns(true);

            // Act
            var result = await _service.ReplyAppointment(input);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            appointment.Status.ShouldBe(AppointmentStatus.Refused);
        }




    }
}
