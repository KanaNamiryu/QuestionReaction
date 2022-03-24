using QuestionReaction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    public class RegisterService : IRegisterService
    {
        public Task<bool> RegisterAsync(string name, string mail, string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
