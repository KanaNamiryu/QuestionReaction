using QuestionReaction.Data;
using QuestionReaction.Data.Interfaces;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuestionReaction.Services
{
    /// <inheritdoc/>
    public class RegisterService : IRegisterService
    {
        private readonly AppDbContext _ctx;
        private readonly IHashService _hashService;

        /// <inheritdoc/>
        public RegisterService(AppDbContext ctx, IHashService hashService)
        {
            _ctx = ctx;
            _hashService = hashService;
        }

        /// <inheritdoc/>
        public async Task<int> RegisterAsync(string name, string mail, string login, string password)
        {

            // mdp doit contenir au moins une minuscule, une majuscule, un chiffre, un caractere spécial et faire au moins 8 characteres
            var regex = new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
            var match = regex.IsMatch(password);

            var loginCheck = _ctx.Users.Any(u => u.Login == login);
            var mailCheck = _ctx.Users.Any(u => u.Mail == mail);

            if (loginCheck)
            {
                return 2; // login deja utilisé
            }
            else if (mailCheck)
            {
                return 3; // mail deja utilisé
            }
            else if (!match)
            {
                return 1; // mot de passe non conforme
            }
            else
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    do
                    {
                        name = NameGenerator();
                    } while (name.Equals(_ctx.Users.SingleOrDefault(u => u.Name == name)));
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

                return 0; // tout est ok
            }

        }

        /// <inheritdoc/>
        public string NameGenerator()
        {
            string name;
            var names1 = new List<string>() { "Chat", "Oiseau", "Poisson", "Sanglier", "Chameau", "Lézard", "Tortue" };
            var names2 = new List<string>() { "Étrange", "Amusant", "Botté", "Rapide", "Multicolore", "Rôti", "Templier" };
            Random random = new();

            var i = random.Next(0, 7);
            name = names1[i];
            var j = random.Next(0, 7);
            name += names2[j];
            if (i == 6 && (j == 1 || j == 2 || j == 5))
            {
                name += "e";
            }
            name += random.Next(1000, 10000);
            return name;
        }

    }
}
