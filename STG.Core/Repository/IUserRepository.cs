using STG.Core.Entities;

namespace STG.Core.Repository
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
    }
}