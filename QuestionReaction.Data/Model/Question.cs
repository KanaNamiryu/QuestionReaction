using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Data.Model
{
    public class Question
    {
        #region Base
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(300)]
        public string Description { get; set; }

        public bool MultipleChoices { get; set; }

        public bool IsActive { get; set; }

        public string VoteUid { get; set; }

        public string ResultUid { get; set; }

        public string DisableUid { get; set; }

        #endregion

        // clefs etrangeres
        #region FK
        [Required]
        public int UserId { get; set; }

        #endregion

        // liens hors BDD
        #region C#
        public User User { get; set; }

        public List<Guest> Guests { get; set; }

        public List<Choice> Choices { get; set; }

        public List<Reaction> Reactions { get; set; }

        #endregion

    }
}
