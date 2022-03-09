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
        [Required]
        public int ChoiceId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        #endregion

        // liens hors BDD
        #region C#
        public Choice Choice { get; set; }
        public User User { get; set; }
        public Question Question { get; set; }

        #endregion

    }
}
