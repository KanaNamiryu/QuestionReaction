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
        /// <summary>
        /// Ajout d'un sondage dans la BDD a partir d'un ViewModel
        /// </summary>
        /// <param name="model"></param>
        Task AddPollAsync(UserAddPollsVM model);
        /// <summary>
        /// Récuperation d'un sondage à partir de son uid de vote
        /// </summary>
        /// <param name="uid">Question.VoteUid</param>
        /// <returns>sondage au format Question</returns>
        Task<Question> GetPollByVoteUidAsync(string uid);
        /// <summary>
        /// Créé un guid au format string sans les tirets
        /// </summary>
        /// <returns>guid au format string</returns>
        string AddGuid();

    }
}
