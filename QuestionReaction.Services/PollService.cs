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
                Question = question
            };
            await _ctx.AddAsync(guest);

            await _ctx.SaveChangesAsync();
        }

        public string AddGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        public async Task<List<Guest>> GetGuestsByQuestionId(int questionId)
        {
            return _ctx.Guests
                .Where(g => g.QuestionId == questionId)
                .ToList();
        }

        public async Task<List<Question>> GetPollsByGuestMailAsync(string guestMail)
        {
            return _ctx.Guests
                .Where(g => g.Mail == guestMail)
                .Select(g => g.Question)
                .ToList();
        }
    }
}
