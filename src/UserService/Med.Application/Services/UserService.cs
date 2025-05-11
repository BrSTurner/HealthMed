using Med.Application.Interfaces.Services;
using Med.Application.Models.Dtos;
using Med.Application.Models.Inputs;
using Med.Domain.Entites;
using Med.Domain.Repositories;
using Med.MessageBus;
using Med.MessageBus.Integration.Requests.Users;
using Med.MessageBus.Integration.Responses.Users;
using Med.SharedKernel.DomainObjects;
using Med.SharedKernel.Enumerations;
using Med.SharedKernel.Models;
using Med.SharedKernel.UoW;

namespace Med.Application.Services
{
    public class UserService(
        IMessageBus bus,
        IDoctorRepository doctorRepository,
        IPatientRepository patientRepository,
        IUnitOfWork unitOfWork) : IUserService
    {
        private readonly IMessageBus _bus = bus;
        private readonly IDoctorRepository _doctorRepository = doctorRepository;
        private readonly IPatientRepository _patientRepository = patientRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<DomainResult> CreateUser(CreateUserInput input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var validationResult = input.Validate();

            if (!validationResult.IsValid)
                return DomainResult.Error(validationResult);

            DomainResult? creationResult = null;

            switch (input.Type)
            {
                case UserType.Patient:
                    creationResult = await CreatePatient(input);
                    break;
                case UserType.Doctor:
                    creationResult = await CreateDoctor(input);
                    break;
            }

            if(creationResult != null && !creationResult.IsSuccess)
                return creationResult;

            if(await _unitOfWork.SaveChanges())
                return DomainResult.Success(creationResult?.Data);

            return DomainResult.Error("Nao foi possivel criar o usuario");
        }        

        private async Task<DomainResult> CreateDoctor(CreateUserInput input)
        {
            var existentDoctor = await _doctorRepository.GetDoctorByCRM(input.CRM ?? string.Empty);

            if (existentDoctor != null)
                return DomainResult.Error("CRM já registrado anteriormente");

            var request = CreateAuthenticationUserRequest(input);
            var response = await CreateAuthenticationUser(request);

            if (response != null && response.Success)
            {
                var doctor = new Doctor
                {
                    UserId = response.UserId,
                    Name = input.Name,
                    SpecialityId = input.SpecialityId ?? Guid.Empty,
                    CRM = CRM.Create(input.CRM ?? string.Empty),
                };

                await _doctorRepository.AddAsync(doctor);
            }

            return CreateResponse(response);
        }

        private async Task<DomainResult> CreatePatient(CreateUserInput input)
        {
            var existentPatient = await _patientRepository.GetPatientByCPFAsync(input.CPF ?? string.Empty);

            if (existentPatient != null)
                return DomainResult.Error("CPF já registrado anteriormente");

            existentPatient = await _patientRepository.GetPatientByEmailAsync(input.Email ?? string.Empty);

            if (existentPatient != null)
                return DomainResult.Error("E-mail já registrado anteriormente");

            var request = CreateAuthenticationUserRequest(input);
            var response = await CreateAuthenticationUser(request);

            if (response != null && response.Success)
            {
                var patient = new Patient
                {
                    UserId = response.UserId,
                    Name = input.Name,
                    CPF = CPF.Create(input.CPF ?? string.Empty),
                    Email = Email.Create(input.Email),
                };

                await _patientRepository.AddAsync(patient);
            }

            return CreateResponse(response);
        }

        public async Task<PatientDTO?> GetPatientByCpf(CPF cpf)
        {
            var entity = await _patientRepository.GetPatientByCPFAsync(cpf.Number);
            if (entity == null)
            {
                return null;
            }

            return MapPatientDTO(entity);
        }

        public async Task<PatientDTO?> GetPatientById(Guid id)
        {
            var entity = await _patientRepository.GetPatientByIdAsync(id);
            if (entity == null)
            {
                return null;
            }

            return MapPatientDTO(entity);
        }

        public async Task<DoctorDTO?> GetDoctorById(Guid id)
        {
            var entity = await _doctorRepository.GetDoctorById(id);
            if (entity == null)
            {
                return null;
            }

            return MapDoctorDTO(entity);
        }

        public async Task<DoctorDTO?> GetDoctorByCrm(CRM crm)
        {
            var entity = await _doctorRepository.GetDoctorByCRM(crm.Number);
            if (entity == null)
            {
                return null;
            }

            return MapDoctorDTO(entity);
        }

        private static CreateUserRequest CreateAuthenticationUserRequest(CreateUserInput input)
        {
            return new CreateUserRequest 
            {
                Email = input.Email,
                Username = (input.Type == UserType.Patient ? input.CPF : input.CRM) ?? string.Empty,
                Password = input.Password,
                Type = input.Type
            };
        }

        public static DoctorDTO MapDoctorDTO(Doctor entity)
        {
            return new DoctorDTO
            {
                Id = entity.Id,
                CRM = entity.CRM,
                SpecialityId = entity.SpecialityId,
                Name = entity.Name,
            };
        }

        private static PatientDTO MapPatientDTO(Patient entity)
        {
            return new PatientDTO
            {
                Id = entity.Id,
                CPF = entity.CPF,
                Email = entity.Email,
                Name = entity.Name,
            };
        }

        private static DomainResult CreateResponse(CreateUserResponse? response) 
        {
            return response != null ? 
                DomainResult.Create(response.Success, [response.ErrorMessage ?? string.Empty], response.Success ? response : null) :
                DomainResult.Error("Nao foi possivel criar o usuario");
        }

        private async Task<CreateUserResponse> CreateAuthenticationUser(CreateUserRequest request)
            => await _bus.RequestAsync<CreateUserRequest, CreateUserResponse>(request);
    }
}
