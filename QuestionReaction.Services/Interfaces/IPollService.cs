using QuestionReaction.Data.Model;
using QuestionReaction.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    /// <summary>
    /// Service de gestion des sondages (ajouts, recuperations de données, ...)
    /// </summary>
    public interface IPollService
    {
        #region Get
        #region Get Question

        /// <summary>
        /// Renvoi le sondage par son id
        /// </summary>
        /// <param name="questionId">id du sondage</param>
        /// <returns></returns>
        Task<Question> GetQuestionByIdAsync(int questionId);
        /// <summary>
        /// Renvoi la liste des sondages créé par un utilisateur
        /// </summary>
        /// <param name="userId">id de l'utilisateur</param>
        /// <returns></returns>
        Task<List<Question>> GetQuestionsByUserIdAsync(int userId);
        /// <summary>
        /// Renvoi la liste des sondages auxquels un mail à été invité
        /// </summary>
        /// <param name="guestMail">mail invité</param>
        /// <returns></returns>
        Task<List<Question>> GetQuestionsByGuestMailAsync(string guestMail);
        /// <summary>
        /// Renvoi un sondage par son uid de vote
        /// </summary>
        /// <param name="voteUid">Uid de vote du sondage</param>
        /// <returns></returns>
        Task<Question> GetQuestionByVoteUidAsync(string voteUid);
        /// <summary>
        /// Renvoi un sondage par son uid de resultat
        /// </summary>
        /// <param name="resultUid">Uid de resultat du sondage</param>
        /// <returns></returns>
        Task<Question> GetQuestionByResultUidAsync(string resultUid);

        #endregion
        #region Get Choice

        /// <summary>
        /// Renvoi un choix par son id
        /// </summary>
        /// <param name="choiceId">id du choix</param>
        /// <returns></returns>
        Task<Choice> GetChoiceByIdAsync(int choiceId);

        #endregion
        #region Get Reaction

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        Task<List<Reaction>> GetReactionsByQuestionIdAsync(int questionId);

        #endregion
        #endregion

        // -----

        #region Ajout en BDD

        /// <summary>
        /// Ajout d'un sondage dans la BDD a partir d'un ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <returns>renvoie l'id du sondage ajouté</returns>
        Task<int> AddPollAsync(UserAddPollsVM model);
        /// <summary>
        /// Ajoute un vote à un sondage
        /// </summary>
        /// <param name="choicesId">liste des id des choix selectionnés par l'utilisateur lors de son vote</param>
        /// <param name="userId">id de l'utilisateur qui vote</param>
        /// <returns></returns>
        Task<string> AddReactionsAsync(List<int> choicesId, int userId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mails"></param>
        /// <param name="questionId"></param>
        /// <returns></returns>
        Task AddGuestsAsync(List<string> mails, int questionId);

        #endregion

        // -----

        #region Autres

        /// <summary>
        /// Créé un guid au format string sans les tirets
        /// </summary>
        /// <returns>guid au format string</returns>
        string AddGuid();
        /// <summary>
        /// Désactive un sondage
        /// </summary>
        /// <param name="disableUid">Uid de désactivation du sondage</param>
        /// <returns></returns>
        Task DisableQuestionAsync(string disableUid);
        /// <summary>
        /// Renvoi la liste des choix d'un sondage triés par ordre décroissant de nombre de votes
        /// </summary>
        /// <param name="questionId">id du sondage</param>
        /// <returns></returns>
        Task<List<Choice>> SortChoicesByVoteNumberAsync(int questionId);
        /// <summary>
        /// Renvoi un booleen indiquant si l'utilisateur à déja voté au sondage (true = à voté)
        /// </summary>
        /// <param name="userId">id de l'utilisateur</param>
        /// <param name="questionId">id du sondage</param>
        /// <returns>Renvoie True si l'utilisateur à déja voté à ce sondage</returns>
        Task<bool> AsAlreadyVotedAsync(int userId, int questionId);
        /// <summary>
        /// Renvoi un booleen selon l'existance de l'uid de vote dans la BDD
        /// </summary>
        /// <param name="voteUid">Uid de vote</param>
        /// <returns>Renvoi True si l'uid existe</returns>
        Task<bool> VoteUidExistsAsync(string voteUid);

        #endregion
    }
}
