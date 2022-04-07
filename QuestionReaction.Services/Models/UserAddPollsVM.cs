using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Models
{
    public class UserAddPollsVM
    {
        [Required(ErrorMessage = "Veuillez choisir un titre"),
            Display(Name = "Titre"),
            MaxLength(100)]
        public string Title { get; set; }
        [Display(Name = "Description"),
            MaxLength(300)]
        public string Description { get; set; }
        [Display(Name = "Choix multiple")]
        public bool MutipleChoices { get; set; }
        public List<string> Choices { get; set; }

        public int CurrentUserId { get; set; }

    }
}
