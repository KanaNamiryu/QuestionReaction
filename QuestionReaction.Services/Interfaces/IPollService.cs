using QuestionReaction.Data.Model;
using QuestionReaction.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    public interface IPollService
    {
        Task AddPollAsync(UserAddPollsVM model);
        Task<Question> GetPollByVoteUidAsync(string uid);
        string AddGuid();

    }
}
