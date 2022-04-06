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
    public class RegisterService : IRegisterService
    {
        private readonly AppDbContext _ctx;
        private readonly IHashService _hashService;

        public RegisterService(AppDbContext ctx, IHashService hashService)
        {
            _ctx = ctx;
            _hashService = hashService;
        }

        public async Task<bool> RegisterAsync(string name, string mail, string login, string password)
        {

            // mdp doit contenir au moins une minuscule, une majuscule, un chiffre, un caractere spécial et faire au moins 8 characteres
            var regex = new Regex("(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^A-Za-z0-9])(?=.{8,})");
            var match = regex.IsMatch(password);

            var l = _ctx.Users.SingleOrDefault(u => u.Login == login);
            var m = _ctx.Users.SingleOrDefault(u => u.Mail == mail);

            if (l == null && m == null && match) // si le login et le mail n'existent pas dans la BDD ET le password est suffisamment sécurisé
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

                return true;
            }

            return false;
        }

        public string NameGenerator()
        {
            string name;
            var names1 = new List<string>() { "Chat", "Oiseau", "Poisson", "Sanglier", "Chameau", "Lézard", "Tortue" };
            var names2 = new List<string>() { "Étrange", "Amusant", "Botté", "Rapide", "Multicolore", "Rôti", "Templier" };
            Random random = new Random();

            var i = random.Next(0, 7);
            name = names1[i];
            var j = random.Next(0, 7);
            name = name + names2[j];
            if (i == 6 && (j == 1 || j == 2 || j == 5))
            {
                name = name + "e";
            }
            name = name + random.Next(1000, 10000);
            return name;
        }

    }
}
