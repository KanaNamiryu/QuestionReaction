using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Data.Model
{
    public class Guest
    {
        #region Base
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }

        #endregion

        // clefs etrangeres
        #region FK
        [Required]
        public int QuestionId { get; set; }

        #endregion

        // liens hors BDD
        #region C#
        public Question Question { get; set; }

        #endregion
    }
}
