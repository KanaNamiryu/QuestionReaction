using QuestionReaction.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Interfaces
{
    /// <summary>
    /// Service de connexion/deconnexion
    /// </summary>
    public interface ILoginService
    {
        /// <summary>
        /// Connecte un internaute si il a des acces valides
        /// </summary>
        /// <param name="login">identifiant de l'internaute</param>
        /// <param name="password">mot de passe de l'internaute</param>
        /// <param name="rememberMe">case à cocher permettant à l'internaute qu'il souhaite rester connecter</param>
        /// <returns></returns>
        Task<bool> LoginAsync(string login, string password, bool rememberMe);

        /// <summary>
        /// Déconnecte l'utilisateur
        /// </summary>
        /// <returns></returns>
        Task LogoutAsync();

        /// <summary>
        /// Renvoi l'utilisateur actuellement connecté
        /// </summary>
        /// <returns>Task de User</returns>
        Task<User> GetCurrentUserAsync();

    }
}
