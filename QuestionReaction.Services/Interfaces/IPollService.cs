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
        Task<int> AddPollAsync(UserAddPollsVM model);
        /// <summary>
        /// Créé un guid au format string sans les tirets
        /// </summary>
        /// <returns>guid au format string</returns>
        string AddGuid();
        /// <summary>
        /// Récupération des invités d'un sondage
        /// </summary>
        /// <param name="questionId">id du sondage</param>
        /// <returns>Liste de Guest</returns>
        Task<List<Guest>> GetGuestsByQuestionId(int questionId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guestMail"></param>
        /// <returns></returns>
        Task<List<Question>> GetQuestionsByGuestMailAsync(string guestMail);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        Task<Question> GetQuestionByIdAsync(int questionId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="guestMail"></param>
        /// <returns></returns>
        Task<List<Question>> GetQuestionsByGuestAsync(string guestMail);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<Question>> GetQuestionsByUserIdAsync(int userId);

        Task DisableQuestionAsync(string disableUid);

        Task<Question> GetQuestionByVoteUidAsync(string voteUid);

        Task<Question> GetQuestionByResultUidAsync(string resultUid);

        Task<Choice> GetChoiceByIdAsync(int choiceId);

        Task<string> AddReactionsAsync(List<int> choicesId, int userId);
    }
}
