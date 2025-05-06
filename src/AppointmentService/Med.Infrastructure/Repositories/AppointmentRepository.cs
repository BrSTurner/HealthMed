using Med.Domain.Entities;
using Med.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Med.Domain.Repositories
{
    public class AppointmentRepository(AppointmentContext context) : IAppointmentRepository
    {
        protected readonly AppointmentContext _context;
        protected readonly DbSet<Appointment> _entity;

        public async Task<List<Appointment>> GetAppointmentsByDoctor(Guid doctorId)
        {
            return await _entity.Where(x => x.DoctorId == doctorId).ToListAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid id)
        {
            return await _entity.FindAsync(id);
        }

        public async void AddAppointmentAsync(Appointment doctor)
        {
            await _entity.AddAsync(doctor);
        }
    }
}
