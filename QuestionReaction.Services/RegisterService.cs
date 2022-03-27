using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using QuestionReaction.Data;
using QuestionReaction.Data.Interfaces;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    public class RegisterService : IRegisterService
    {
        public RegisterService(AppDbContext ctx, IHashService hashService)
        {
            _ctx = ctx;
            _hashService = hashService;
        }

        private readonly AppDbContext _ctx;
        private readonly IHashService _hashService;

        public async Task<bool> RegisterAsync(string name, string mail, string login, string password)
        {
            var l = _ctx.Users.SingleOrDefault(u => u.Login == login);
            var m = _ctx.Users.SingleOrDefault(u => u.Mail == mail);
            if (l == null && m == null)
            {
                if (name == null)
                {
                    name = "RandomUser"; // Nom a generer
                }

                var user = new User
                {
                    Name = name,
                    Mail = mail,
                    Login = login,
                    Password = _hashService.HashPassword(password)
                };
                await _ctx.AddAsync(user);
                await _ctx.SaveChangesAsync();

                return true;
            }

            return false;
        }

    }
}
