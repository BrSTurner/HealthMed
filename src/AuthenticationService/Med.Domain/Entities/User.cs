using FluentValidation.Results;
using Med.Domain.Enumerations;
using Med.Domain.Validations;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Enumerations;

namespace Med.Domain.Entities
{
    public class User : Entity, IAggregateRoot
    {
        public required string Username { get; set; }
        public Email? Email { get; set; }
        public required string PasswordHash { get; set; }
        public UserType Type { get; set; }
        public ICollection<UserRole>? Roles { get; set; }

        public override ValidationResult Validate()
            => new UserValidation().Validate(this);

        public void AddRoles()
        {
            switch (Type)
            {
                case UserType.Doctor:
                    AddDoctorRoles();
                    break;
                case UserType.Patient:
                    AddPatientRoles();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private void AddDoctorRoles()
            => Roles = [new() { RoleId = (int)RolesEnum.Doctor }];

        private void AddPatientRoles()
            => Roles = [new() { RoleId = (int)RolesEnum.Patient }];
    }
}
