using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using QuestionReaction.Data;
using QuestionReaction.Data.Interfaces;

namespace QuestionReaction.Services
{
    public class LoginService : ILoginService
    {
        public LoginService(IHttpContextAccessor contextAccessor, AppDbContext ctx, IHashService hashService)
        {
            _httpContext = contextAccessor.HttpContext;
            _ctx = ctx;
            _hashService = hashService;
        }

        private readonly HttpContext _httpContext;
        private readonly AppDbContext _ctx;
        private readonly IHashService _hashService;

        public async Task<bool> LoginAsync(string login, string password, bool rememberMe)
        {
            // check du login + mdp
            var u = _ctx.Users.SingleOrDefault(u => u.Login == login);
            if (u != null)
            {
                var hash = _hashService.HashPassword(password);
                if (u.Password == hash)
                {
                    // enregistrer les infos sous forme de claim
                    var claims = new List<Claim>
                    {
                        new Claim("name", u.Name),
                        new Claim("id", u.Id.ToString())
                    };

                    // mettre la liste de claim dans une identity, puis dans un ClaimsPrincipal qui servira a se connecter
                    var identity = new ClaimsIdentity(claims, "Cookies");
                    var principal = new ClaimsPrincipal(identity);
                    await _httpContext.SignInAsync(principal, new AuthenticationProperties
                    {
                        IsPersistent = rememberMe
                    });
                    return true;
                }
            }

            return false;
        }

        public async Task LogoutAsync()
        {
            await _httpContext.SignOutAsync();
        }

        public async Task<User> GetCurrentUserAsync()
        {
            if (_httpContext.User.Claims.FirstOrDefault() == null)
            {
                return null;
            }
            else
            {
                var userId = _httpContext.User.Claims.Single(u => u.Type == "id").Value;
                var user = _ctx.Users.Where(u => u.Id == int.Parse(userId)).FirstOrDefault();
                return user;
            }
        }
    }
}
