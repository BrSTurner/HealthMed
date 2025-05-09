using Med.Domain.Entities;
using Med.SharedKernel.Repositories;

namespace Med.Domain.Repositories
{
    public interface IAppointmentRepository : IRepository<Appointment>
    {
        void AddAppointmentAsync(Appointment doctor);

        Task<Appointment?> GetAppointmentByIdAsync(Guid id);

        Task<List<Appointment>> GetAppointmentsByDoctor(Guid doctorId);
    }
}
