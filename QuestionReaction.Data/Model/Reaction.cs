using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Data.Model
{
    public class Reaction
    {
        #region Base
        public int Id { get; set; }

        #endregion

        // clefs etrangeres
        #region FK
        // clefs non requises et nullable car seul moyen d'éviter que EF ne considère de boucles entre les tables
        // erreur en question : "Introducing FOREIGN KEY constraint [...] may cause cycles or multiple cascade paths. [...]"
        public int? ChoiceId { get; set; }
        public int? UserId { get; set; }
        public int? QuestionId { get; set; }
        #endregion

        // liens hors BDD
        #region C#
        public Choice Choice { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }

        #endregion

    }
}
