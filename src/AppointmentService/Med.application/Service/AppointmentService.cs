using Med.Application.Models;
using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Appointments;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
using Med.SharedKernel.Models;
using Med.SharedKernel.UoW;

namespace Med.Application.Services
{
    public class AppointmentService(IMessageBus bus,
        IAppointmentRepository appointmentRepository,
        IUnitOfWork unitOfWork) : IAppointmentService
    {
        private readonly IMessageBus _bus = bus;
        private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<DomainResult> CancelAppointment(CancelAppointmentInput input)
        {
            var validationResult = input.Validate();

            if (!validationResult.IsValid)
                DomainResult.Error(validationResult);

            var checkCalendarRequest = new UpdateCalendarAppointmentRequest
            {
                BookingTimeId = input.BookingTimeId,
                IsCancelled = true
            };

            var calendarResponse = await UpdateCalendarIfAvailable(checkCalendarRequest);
            if (!calendarResponse.Success)
            {
                return DomainResult.Error(calendarResponse.ErrorMessage);
            }

            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(input.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Canceled;
                appointment.ReasonForCanceling = input.ReasonForCanceling;
            }

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success();


            return DomainResult.Error("Nao foi possivel cancelar a consulta");
        }

        public async Task<DomainResult> CreateAppointment(CreateAppointmentInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var validationResult = input.Validate();

            if (!validationResult.IsValid)
                DomainResult.Error(validationResult);

            var getDoctorRequest = new GetDoctorByAppointmentRequest
            {
                DoctorId = input.DoctorId
            };

            var doctorResponse = await GetDoctorByAppointment(getDoctorRequest);
            if (!doctorResponse.Success)
            {
                return DomainResult.Error(doctorResponse.ErrorMessage);
            }

            var checkCalendarRequest = new UpdateCalendarAppointmentRequest
            {
                BookingTimeId = input.BookingTimeId
            };

            var calendarResponse = await UpdateCalendarIfAvailable(checkCalendarRequest);
            if (!calendarResponse.Success)
            {
                return DomainResult.Error(calendarResponse.ErrorMessage);
            }

            var appointment = new Appointment
            {
                DoctorId = doctorResponse.DoctorId,
                PatientId = input.PatientId,
                Date = input.Date,
                Status = AppointmentStatus.Pending
            };

            _appointmentRepository.AddAppointmentAsync(appointment);

            var dto = MapAppointmentDTO(appointment);

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success(dto);


            return DomainResult.Error("Nao foi possivel criar a consulta");
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByDoctor(Guid doctorId)
        {
            var appointments = await _appointmentRepository.GetAppointmentsByDoctor(doctorId);

            return appointments.Select(x => MapAppointmentDTO(x)).ToList();
        }

        public async Task<DomainResult> ReplyAppointment(ReplyAppointmentInput input)
        {
            var validationResult = input.Validate();

            if (!validationResult.IsValid)
                DomainResult.Error(validationResult);

            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(input.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = input.Status;
            }

            if(appointment.Status == AppointmentStatus.Refused)
            {
                var checkCalendarRequest = new UpdateCalendarAppointmentRequest
                {
                    BookingTimeId = input.BookingTimeId,
                    IsCancelled = true
                };

                var calendarResponse = await UpdateCalendarIfAvailable(checkCalendarRequest);
                if (!calendarResponse.Success)
                {
                    return DomainResult.Error(calendarResponse.ErrorMessage);
                }
            }

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success();


            return DomainResult.Error("Nao foi possivel responder a consulta");
        }

        private AppointmentDTO MapAppointmentDTO(Appointment entity)
        {
            return new AppointmentDTO
            {
                Id = entity.Id,
                Date = entity.Date,
                PatientId = entity.PatientId,
                DoctorId = entity.DoctorId,
                Status = entity.Status,
            };
        }

        private async Task<GetDoctorByAppointmentResponse> GetDoctorByAppointment(GetDoctorByAppointmentRequest request)
            => await _bus.RequestAsync<GetDoctorByAppointmentRequest, GetDoctorByAppointmentResponse>(request);

        private async Task<UpdateCalendarAppointmentResponse> UpdateCalendarIfAvailable(UpdateCalendarAppointmentRequest request)
            => await _bus.RequestAsync<UpdateCalendarAppointmentRequest, UpdateCalendarAppointmentResponse>(request);
    }
}
