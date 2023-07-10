using STG.Core.Entities;

namespace STG.Core.Services
{
    public interface IUserService
    {
        Task<User> GetUser(string username, string password);
        bool ValidateCredentials(User user, string password);
    }
}