using QuestionReaction.Data.Model;
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
        [Required,
            Display(Name = "Titre"),
            MaxLength(100)]
        public string Title { get; set; }
        [Display(Name = "Description"),
            MaxLength(300)]
        public string Description { get; set; }
        [Display(Name = "Choix multiple")]
        public bool MutipleChoices { get; set; }
        [Required,
            Display(Name = "Liste des choix")]
        public List<Choice> Choices { get; set; }

        public string ReturnUrl { get; set; }
    }
}
