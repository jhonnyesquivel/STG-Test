using STG.Core.Entities;
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

        public async Task<User> GetUser(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            return user;
        }

        public bool ValidateCredentials(User user, string password)
        {
            return password == user.Password;
        }
    }
}