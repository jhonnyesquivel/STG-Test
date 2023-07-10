using STG.Core.Repository;
using STG.Core.Services;

namespace STG.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            //return PasswordHasher.VerifyPassword(password, user.Password);
            return password == user.Password;
        }
    }
}