using Microsoft.EntityFrameworkCore;
using QuestionReaction.Data;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using QuestionReaction.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    /// <inheritdoc/>
    public class PollService : IPollService
    {
        private readonly AppDbContext _ctx;
        private readonly IUserService _userService;
        /// <summary>
        /// Constructeur du service, appelle les autres services utilisés et initialise ses propriétés
        /// </summary>
        /// <param name="ctx">Permet la lecture et écriture en base de donnée</param>
        /// <param name="userService">Permet d'effectuer des actions relatives au utilisateurs</param>
        public PollService(AppDbContext ctx, IUserService userService)
        {
            _ctx = ctx;
            _userService = userService;
        }

        #region Get
        #region Get Question

        /// <inheritdoc/>
        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _ctx.Questions
                .Include(q => q.Reactions)
                .Include(q => q.User)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        /// <inheritdoc/>
        public async Task<List<Question>> GetQuestionsByUserIdAsync(int userId)
        {
            return await _ctx.Questions
                .Where(q => q.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Question>> GetQuestionsWithReactionsByUserIdAsync(int userId)
        {
            return await _ctx.Questions
                .Include(q => q.Reactions)
                .Where(q => q.UserId == userId)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Question>> GetQuestionsByGuestMailAsync(string guestMail)
        {
            return await _ctx.Guests
                .Include(g => g.Question)
                .ThenInclude(q => q.Reactions) // include la liste des reaction de la question qu'on souhaite récupérer
                .Where(g => g.Mail == guestMail)
                .Select(g => g.Question)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<Question> GetQuestionByVoteUidAsync(string voteUid)
        {
            return await _ctx.Questions
                .Include(q => q.Choices)
                .Include(q => q.Reactions)
                .FirstOrDefaultAsync(q => q.VoteUid == voteUid);
        }

        /// <inheritdoc/>
        public async Task<Question> GetQuestionByResultUidAsync(string resultUid)
        {
            return await _ctx.Questions
                .Include(q => q.Choices)
                .Include(q => q.Reactions)
                .FirstOrDefaultAsync(q => q.ResultUid == resultUid);
        }

        #endregion
        #region Get Choice

        /// <inheritdoc/>
        public async Task<Choice> GetChoiceByIdAsync(int choiceId)
        {
            return await _ctx.Choices
                .FirstOrDefaultAsync(c => c.Id == choiceId);
        }

        #endregion
        #region Get Reaction

        /// <inheritdoc/>
        public async Task<List<Reaction>> GetReactionsByQuestionIdAsync(int questionId)
        {
            return await _ctx.Reactions
                .Include(r => r.User)
                .Where(r => r.QuestionId == questionId)
                .ToListAsync();
        }

        #endregion
        #endregion

        // -----

        #region Ajout en BDD

        /// <inheritdoc/>
        public async Task<int> AddPollAsync(UserAddPollsVM model)
        {
            var question = new Question
            {
                Title = model.Title,
                Description = model.Description,
                MultipleChoices = model.MutipleChoices,
                IsActive = true,
                VoteUid = AddGuid(),
                ResultUid = AddGuid(),
                DisableUid = AddGuid(),
                UserId = model.CurrentUserId,
                Choices = model.Choices
                    .Select(c => new Choice
                    {
                        Content = c
                    })
                    .ToList()
            };
            await _ctx.AddAsync(question);

            var userMail = _userService.GetUserByIdAsync(model.CurrentUserId).Result.Mail;
            var guest = new Guest
            {
                Mail = userMail,
                Question = question,
                Date = DateTime.Now
            };
            await _ctx.AddAsync(guest);
            await _ctx.SaveChangesAsync();

            question = await GetQuestionByVoteUidAsync(question.VoteUid);

            return question.Id;
        }

        /// <inheritdoc/>
        public async Task<string> AddReactionsAsync(List<int> choicesId, int userId)
        {
            var choices = choicesId
                .Select(c => GetChoiceByIdAsync(c).Result)
                .ToList();

            var user = await _userService.GetUserByIdAsync(userId);

            var question = await GetQuestionByIdAsync(choices.FirstOrDefault().QuestionId);

            var reactions = new List<Reaction>();

            choices.ForEach(c => reactions
                .Add(new Reaction()
                {
                    Choice = c,
                    User = user,
                    Question = question
                }));

            await _ctx.AddRangeAsync(reactions);
            await _ctx.SaveChangesAsync();

            return question.ResultUid;
        }

        /// <inheritdoc/>
        public async Task AddGuestsAsync(List<string> mails, int questionId)
        {
            var question = await GetQuestionByIdAsync(questionId);

            var guests = new List<Guest>();

            var validMails = mails
                .Where(m => IsvalidMail(m))
                .ToList();

            validMails.ForEach(m => guests
            .Add(new Guest()
            {
                Mail = m,
                Question = question,
                Date = DateTime.Now
            }));

            await _ctx.AddRangeAsync(guests);
            await _ctx.SaveChangesAsync();

        }

        #endregion

        // -----

        #region Autres

        /// <inheritdoc/>
        public string AddGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        /// <inheritdoc/>
        public async Task DisableQuestionAsync(string disableUid, int currentUserId)
        {
            var question = await _ctx.Questions
                .FirstOrDefaultAsync(q => q.DisableUid == disableUid);
            if (question.UserId != currentUserId)
            {
                return;
            }
            question.IsActive = false;
            _ctx.Questions.Update(question);
            await _ctx.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<List<Choice>> SortChoicesByVoteNumberAsync(int questionId)
        {
            var choices = await _ctx.Choices
                .Include(c => c.Question)
                .Include(c => c.Reactions)
                .Where(c => c.Question.Id == questionId)
                .ToListAsync();

            var sortedChoices = choices
                .OrderByDescending(c => c.Reactions.Count)
                .ToList();

            return sortedChoices;
        }

        /// <inheritdoc/>
        public async Task<bool> AsAlreadyVotedAsync(int userId, int questionId)
        {
            var reactions = await GetReactionsByQuestionIdAsync(questionId);

            return reactions.Any(r => r.User.Id == userId);
        }

        /// <inheritdoc/>
        public async Task<bool> VoteUidExistsAsync(string voteUid)
        {
            return await _ctx.Questions.AnyAsync(q => q.VoteUid == voteUid);
        }

        private static bool IsvalidMail(string mail)
        {
            try
            {
                var m = new MailAddress(mail);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        #endregion

    }
}
