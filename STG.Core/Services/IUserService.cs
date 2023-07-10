namespace STG.Core.Services
{
    public interface IUserService
    {
        Task<bool> ValidateCredentials(string username, string password);
    }
}