using QuestionReaction.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    public class LoginService : ILoginService
    {
        public Task<bool> LoginAsync(string login, string password, bool rememberMe)
        {
            return Task.FromResult(true);
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
