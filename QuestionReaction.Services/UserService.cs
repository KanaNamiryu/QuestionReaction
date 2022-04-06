using Microsoft.EntityFrameworkCore;
using QuestionReaction.Data;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _ctx;
        public UserService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _ctx.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

    }
}
