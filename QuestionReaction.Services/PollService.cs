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
        public PollService(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task AddPollAsync(UserAddPollsVM model)
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
                UserId = model.CurrentUserId
            };

            await _ctx.AddAsync(question);

            // save question avant de la chercher OU add choices dans la liste de choices de la question

            var questionId = GetPollByVoteUidAsync(question.VoteUid).Id;

            model.Choices
                .Select(async c => await _ctx
                    .AddAsync(new Choice
                        {
                            Content = c,
                            QuestionId = questionId
                        }));
            
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
