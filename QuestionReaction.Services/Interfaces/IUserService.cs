using QuestionReaction.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Renvoi un utilisateur par son id
        /// </summary>
        /// <param name="userId">id de l'utilisateur</param>
        /// <returns>Task de User</returns>
        Task<User> GetUserByIdAsync(int userId);
    }
}
