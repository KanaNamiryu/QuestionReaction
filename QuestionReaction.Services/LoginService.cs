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
using System.Security.Cryptography;
using QuestionReaction.Data;

namespace QuestionReaction.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpContext _httpContext;
        private readonly AppDbContext _ctx;

        public LoginService(IHttpContextAccessor contextAccessor, AppDbContext ctx)
        {
            _httpContext = contextAccessor.HttpContext;
            _ctx = ctx;
        }

        public async Task<bool> LoginAsync(string login, string password, bool rememberMe)
        {
            // check du login + mdp
            var u = _ctx.Users.SingleOrDefault(u => u.Login == login);
            if (u != null)
            {
                var hash = HashPassword(password);
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

        private string HashPassword(string clearPwd)
        {
            if (string.IsNullOrEmpty(clearPwd))
                return string.Empty;

            byte[] tmpSource;
            byte[] tmpHash;

            tmpSource = Encoding.ASCII.GetBytes(clearPwd);
            using (var sha = SHA256.Create())
            {
                tmpHash = sha.ComputeHash(tmpSource);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < tmpHash.Length; i++)
                {
                    sb.Append(tmpHash[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
