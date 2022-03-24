using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Data.Model
{
    public class User
    {
        #region Base
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        [Required]
        [MaxLength(50)]
        public string Login { get; set; }

        [Required]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion

        // liens hors BDD
        #region C#
        public List<Question> Questions { get; set; }

        public List<Reaction> Reactions { get; set; }

        #endregion

    }
}
