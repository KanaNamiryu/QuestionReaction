using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Data.Interfaces
{
    public interface IHashService
    {
        string HashPassword(string clearPwd);
    }
}
