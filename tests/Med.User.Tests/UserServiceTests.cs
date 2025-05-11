using FluentValidation.Results;
using Med.Application.Models.Inputs;
using Med.Application.Services;
using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Users;
using Med.MessageBus.Integration.Responses.Users;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Enumerations;
using Med.SharedKernel.UoW;
using NSubstitute;
using Shouldly;

namespace Med.User.Tests
{
    public class UserServiceTests
    {
        private readonly IMessageBus _bus = Substitute.For<IMessageBus>();
        private readonly IDoctorRepository _doctorRepo = Substitute.For<IDoctorRepository>();
        private readonly IPatientRepository _patientRepo = Substitute.For<IPatientRepository>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
        private readonly UserService _service;

        public UserServiceTests()
        {
            _service = new UserService(_bus, _doctorRepo, _patientRepo, _unitOfWork);
        }

        [Fact(DisplayName = "Create Invalid User")]
        [Trait("User Service", "Create")]
        public async Task CreateUser_ShouldReturnError_WhenInputIsNull()
        {
            await Should.ThrowAsync<ArgumentNullException>(() => _service.CreateUser(null!));
        }

        [Fact(DisplayName = "Invalid Input User")]
        [Trait("User Service", "Create")]
        public async Task CreateUser_ShouldReturnValidationError_WhenInputIsInvalid()
        {
            var input = Substitute.For<CreateUserInput>();
            input.Validate().Returns(new ValidationResult(new[] { new ValidationFailure("Name", "Required") }));

            var result = await _service.CreateUser(input);

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("Required");
        }

        [Fact(DisplayName = "CRM Exists")]
        [Trait("User Service", "Create")]
        public async Task CreateUser_ShouldReturnError_WhenDoctorCRMExists()
        {
            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                CRM = CRM.Create("12345/SP"),
                Name = "Dr. House",
                SpecialityId = Guid.NewGuid()
            };
            var input = CreateDoctorInput();
            _doctorRepo.GetDoctorByCRM(input.CRM).Returns(doctor);

            var result = await _service.CreateUser(input);

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("CRM já registrado anteriormente");
        }

        [Fact(DisplayName = "CPF and E-mail Exists")]
        [Trait("User Service", "Create")]
        public async Task CreateUser_ShouldReturnError_WhenPatientCPFOrEmailExists()
        {
            var input = CreatePatientInput();
            var patient = new Patient() 
            {
                UserId = Guid.NewGuid(),
                Name = "Bruno",
                Email = Email.Create("test@test.com"),
                CPF = CPF.Create("123.456.789-09") 
            };
            _patientRepo.GetPatientByCPFAsync(input.CPF).Returns(patient);

            var result = await _service.CreateUser(input);

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("CPF já registrado anteriormente");

            // Test email too
            _patientRepo.GetPatientByCPFAsync(input.CPF).Returns((Patient?)null);
            _patientRepo.GetPatientByEmailAsync(input.Email).Returns(patient);

            result = await _service.CreateUser(input);

            result.IsSuccess.ShouldBeFalse();
            result.Errors.ShouldContain("E-mail já registrado anteriormente");
        }

        [Fact(DisplayName = "Create Doctor")]
        [Trait("User Service", "Create")]
        public async Task CreateUser_ShouldReturnSuccess_WhenDoctorCreatedSuccessfully()
        {
            var input = CreateDoctorInput();
            _doctorRepo.GetDoctorByCRM(input.CRM).Returns((Doctor?)null);
            var response = new CreateUserResponse { Success = true, UserId = Guid.NewGuid() };
            _bus.RequestAsync<CreateUserRequest, CreateUserResponse>(Arg.Any<CreateUserRequest>())
                .Returns(response);
            _unitOfWork.SaveChanges().Returns(true);

            var result = await _service.CreateUser(input);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Create Patient")]
        [Trait("User Service", "Create")]
        public async Task CreateUser_ShouldReturnSuccess_WhenPatientCreatedSuccessfully()
        {
            var input = CreatePatientInput();
            _patientRepo.GetPatientByCPFAsync(input.CPF).Returns((Patient?)null);
            _patientRepo.GetPatientByEmailAsync(input.Email).Returns((Patient?)null);
            var response = new CreateUserResponse { Success = true, UserId = Guid.NewGuid() };
            _bus.RequestAsync<CreateUserRequest, CreateUserResponse>(Arg.Any<CreateUserRequest>())
                .Returns(response);
            _unitOfWork.SaveChanges().Returns(true);

            var result = await _service.CreateUser(input);

            result.IsSuccess.ShouldBeTrue();
        }

        [Fact(DisplayName = "Get Doctor By Id")]
        [Trait("User Service", "Get")]
        public async Task GetDoctorById_ShouldReturnMappedDTO_WhenFound()
        {
            var doctor = new Doctor
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(), 
                CRM = CRM.Create("12345/SP"),
                Name = "Dr. House",
                SpecialityId = Guid.NewGuid()
            };

            _doctorRepo.GetDoctorById(doctor.Id).Returns(doctor);

            var result = await _service.GetDoctorById(doctor.Id);

            result.ShouldNotBeNull();
            result?.Name.ShouldBe("Dr. House");
        }

        [Fact(DisplayName = "Get CPF By Id")]
        [Trait("User Service", "Get")]
        public async Task GetPatientByCpf_ShouldReturnNull_WhenNotFound()
        {
            _patientRepo.GetPatientByCPFAsync("123.456.789-09").Returns((Patient?)null);
            var result = await _service.GetPatientByCpf(CPF.Create("123.456.789-09"));
            result.ShouldBeNull();
        }

        private static CreateUserInput CreateDoctorInput()
        {
            return new CreateUserInput
            {
                Name = "Dr. Who",
                CRM = "12345/SP",
                Type = UserType.Doctor,
                Password = "secret",
                SpecialityId = Guid.NewGuid()
            };
        }

        private static CreateUserInput CreatePatientInput()
        {
            return new CreateUserInput
            {
                Name = "Patient Zero",
                CPF = "123.456.789-09",
                Email = "patient@example.com",
                Type = UserType.Patient,
                Password = "secret"
            };
        }
    }
}