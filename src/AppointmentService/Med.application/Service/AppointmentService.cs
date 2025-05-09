using Med.Application.Models;
using Med.Domain.Entities;
using Med.Domain.Enumerations;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Appointments;
using Med.MessageBus.Integration.Requests.Calendars;
using Med.MessageBus.Integration.Responses.Calendars;
using Med.SharedKernel.DomainObjects;
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

        public async void CancelAppointment(CancelAppointmentInput input)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(input.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = AppointmentStatus.Canceled;
                appointment.ReasonForCanceling = input.ReasonForCanceling;
            }

            await _unitOfWork.SaveChanges();
        }

        public async Task<DomainResult> CreateAppointment(CreateAppointmentInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var getDoctorRequest = new GetDoctorByAppointmentRequest
            {
                Crm = new CRM(input.Crm)
            };

            var doctorResponse = await GetDoctorByAppointment(getDoctorRequest);
            if (!doctorResponse.Success)
            {
                return DomainResult.Error(doctorResponse.ErrorMessage);
            }

            var checkCalendarRequest = new UpdateCalendarIfAvailableRequest
            {
                Date = input.Date,
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

            if (await _unitOfWork.SaveChanges())
                return DomainResult.Success();


            return DomainResult.Error("Nao foi possivel criar a consulta");
        }

        public async void ReplyAppointment(ReplyAppointmentInput replyAppointmentInput)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(replyAppointmentInput.AppointmentId);
            if (appointment != null)
            {
                appointment.Status = replyAppointmentInput.Status;
            }

            await _unitOfWork.SaveChanges();
        }

        private async Task<GetDoctorByAppointmentResponse> GetDoctorByAppointment(GetDoctorByAppointmentRequest request)
            => await _bus.RequestAsync<GetDoctorByAppointmentRequest, GetDoctorByAppointmentResponse>(request);

        private async Task<UpdateCalendarIfAvailableResponse> UpdateCalendarIfAvailable(UpdateCalendarIfAvailableRequest request)
            => await _bus.RequestAsync<UpdateCalendarIfAvailableRequest, UpdateCalendarIfAvailableResponse>(request);
    }
}
