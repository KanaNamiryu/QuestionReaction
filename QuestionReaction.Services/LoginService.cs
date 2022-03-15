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

namespace QuestionReaction.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpContext _httpContext;

        public LoginService(IHttpContextAccessor contextAccessor)
        {
            _httpContext = contextAccessor.HttpContext;
        }

        public async Task<bool> LoginAsync(string login, string password, bool rememberMe)
        {
            // check du login + mdp

            // liens avec la bdd
            var user = new User
            {
                Id = 1,
                Name = "Lucas F",
                Login = "lucas",
                Password = "azerty"
            };

            // enregistrer les infos sous forme de claim
            var claims = new List<Claim>
            {
                new Claim("login", user.Login),
                new Claim("name", user.Name),
                new Claim("id", user.Id.ToString())
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

        public async Task LogoutAsync()
        {
            await _httpContext.SignOutAsync();
        }
    }
}
