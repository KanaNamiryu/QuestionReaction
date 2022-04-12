using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    /// <summary>
    /// Service d'inscription d'un nouvel utilisateur
    /// </summary>
    public interface IRegisterService
    {
        /// <summary>
        /// Enregistre un internaute en tant que nouvel utilisateur
        /// </summary>
        /// <param name="name">Nom de l'internaute (généré aléatoirement si non choisi)</param>
        /// <param name="mail">Mail de l'internaute (ne doit pas etre deja inscrit)</param>
        /// <param name="login">Identifiant de l'internaute (ne doit pas etre deja inscrit)</param>
        /// <param name="password">Mot de passe de l'internaute</param>
        /// <returns>True si l'enregistrement s'est bien passé, False sinon</returns>
        Task<bool> RegisterAsync(string name, string mail, string login, string password);

        /// <summary>
        /// Renvoi un pseudo généré aléatoirement
        /// </summary>
        /// <returns>String</returns>
        string NameGenerator();
    }
}
