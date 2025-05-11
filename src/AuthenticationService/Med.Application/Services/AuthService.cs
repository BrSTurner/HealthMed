using Med.Application.Interfaces.Services;
using Med.Domain.Entities;
using Med.Domain.Exceptions;
using Med.Domain.Repositories;
using Med.SharedKernel.UoW;

namespace Med.Application.Services
{
    public class AuthService(
        IUserRepository userRepository, 
        IUnitOfWork unitOfWork,
        IPasswordService passwordService,
        ITokenService tokenService,
        IRoleRepository roleRepository) : IAuthService
    {
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IRoleRepository _roleRepository = roleRepository;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        
        public async Task<Guid> CreateUser(User user)
        {
            ValidateUser(user);

            if(await UserAlreadyExists(user))
                throw new UserAlreadyCreatedException("Usuário já cadastrado anteriormente");

            user.PasswordHash = _passwordService.HashPassword(user.PasswordHash);
            user.AddRoles();

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChanges();

            return user.Id;
        }

        public async Task<string> Authenticate(string usernameOrEmail, string password)
        {
            var user = await _userRepository.GetByEmailOrUsernameAsync(usernameOrEmail, usernameOrEmail) ?? throw new UnauthorizedAccessException("Usuário não foi encontrado");

            if (_passwordService.IsPasswordValid(password, user.PasswordHash) == false)
                throw new UnauthorizedAccessException("Senha incorreta");

            var roles = await _roleRepository.GetRolesById(user.Roles?.Select(x => x.RoleId)?.ToArray() ?? []);

            return _tokenService.GenerateToken(user, roles);
        }

        private static void ValidateUser(User user) 
        {
            ArgumentNullException.ThrowIfNull(user);

            var validationResult = user.Validate();

            if (!validationResult.IsValid)
                throw new UserInvalidDataException(validationResult.Errors.First().ErrorMessage);
        }

        private async Task<bool> UserAlreadyExists(User user)
        {
            var existingUser = await _userRepository.GetByEmailOrUsernameAsync(user.Username, user.Email?.Address ?? string.Empty);
            return existingUser != null;
        }
    }
}
