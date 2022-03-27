using QuestionReaction.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(string login, string password, bool rememberMe);
        Task LogoutAsync();
        Task<User> GetCurrentUserAsync();
    }
}
