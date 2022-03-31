using QuestionReaction.Data;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using QuestionReaction.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    public class PollService : IPollService
    {
        private readonly AppDbContext _ctx;
        private readonly IUserService _userService;
        public PollService(AppDbContext ctx, IUserService userService)
        {
            _ctx = ctx;
            _userService = userService;
        }
        public async Task AddPollAsync(UserAddPollsVM model)
        {
            // creation et ajout du sondage en lui-meme
            var question = new Question
            {
                Title = model.Title,
                Description = model.Description,
                MultipleChoices = model.MutipleChoices,
                IsActive = true,
                VoteUid = AddGuid(),
                ResultUid = AddGuid(),
                DisableUid = AddGuid(),
                UserId = model.CurrentUserId
            };

            await _ctx.AddAsync(question);
            await _ctx.SaveChangesAsync();

            var questionId = GetPollByVoteUidAsync(question.VoteUid).Id;

            /* creation et ajout des choix et du createur du sondage
             en tant que premier guest (pour avoir le droit de voter)
             à partir de l'id de sondage récupéré au dessus */
            model.Choices
                .Select(async c => await _ctx
                    .AddAsync(new Choice
                        {
                            Content = c,
                            QuestionId = questionId
                        }));

            var userMail = _userService.GetUserByIdAsync(model.CurrentUserId).Result.Mail;
            var guest = new Guest
            {
                Mail = userMail,
                QuestionId = questionId
            };
            await _ctx.AddAsync(guest);

            await _ctx.SaveChangesAsync();
        }

        public string AddGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        public async Task<Question> GetPollByVoteUidAsync(string uid)
        {
            var result = _ctx.Questions
                .FirstOrDefault(q => q.VoteUid == uid);

            return result;
        }
    }
}
