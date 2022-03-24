using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    public interface IRegisterService
    {
        Task<bool> RegisterAsync(string name, string mail, string login, string password);
    }
}
