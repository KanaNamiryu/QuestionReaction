using Microsoft.EntityFrameworkCore;
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
                Question = question
            };
            await _ctx.AddAsync(guest);
            await _ctx.SaveChangesAsync();

            question = await GetQuestionByVoteUid(question.VoteUid);

            return question.Id;
        }

        public string AddGuid()
        {
            return Guid.NewGuid().ToString().Replace("-", "").ToUpper();
        }

        public async Task<List<Guest>> GetGuestsByQuestionId(int questionId)
        {
            return await _ctx.Guests
                .Where(g => g.QuestionId == questionId)
                .ToListAsync();
        }

        public async Task<List<Question>> GetQuestionsByGuestMailAsync(string guestMail)
        {
            return await _ctx.Guests
                .Where(g => g.Mail == guestMail)
                .Select(g => g.Question)
                .ToListAsync();
        }

        public async Task<Question> GetQuestionByIdAsync(int questionId)
        {
            return await _ctx.Questions
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        public async Task<List<Question>> GetQuestionsByGuestAsync(string guestMail)
        {
            return _ctx.Guests
                .Where(g => g.Mail == guestMail)
                .ToList()
                .Select(g => g.Question)
                .ToList();
        }

        public async Task<List<Question>> GetQuestionsByUserIdAsync(int userId)
        {
            return await _ctx.Users
                .Where(u => u.Id == userId)
                .Select(u => u.Questions)
                .FirstOrDefaultAsync();
        }

        public async Task DisableQuestionAsync(string disableUid)
        {
            var question = await _ctx.Questions
                .FirstOrDefaultAsync(q => q.DisableUid == disableUid);
            question.IsActive = false;
            _ctx.Questions.Update(question);
            await _ctx.SaveChangesAsync();
        }

        public async Task<Question> GetQuestionByVoteUid(string voteUid)
        {
            return await _ctx.Questions
                .FirstOrDefaultAsync(q => q.VoteUid == voteUid);
        }
    }
}
