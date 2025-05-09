using Med.Domain.Entities;
using Med.Domain.Enumerations;

namespace Med.Infrastructure.Seeding
{
    public static class RoleSeeding
    {
        public static Role[] Roles =>
        [
            new Role { Id = (int)RolesEnum.Admin, Name = "Admin", CreatedAt = new DateTime(2025, 05, 05, 00, 00, 00) },
            new Role { Id = (int)RolesEnum.Doctor, Name = "Doctor", CreatedAt = new DateTime(2025, 05, 05, 00, 00, 00) },
            new Role { Id = (int)RolesEnum.Patient, Name = "Patient", CreatedAt = new DateTime(2025, 05, 05, 00, 00, 00) }
        ];
    }
}
