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

        #region Choices
        [Required(ErrorMessage = "Veuillez définir les 2 premiers choix au minimum"),
            Display(Name = "Choix 1")]
        public string Choice_1 { get; set; }
        [Required(ErrorMessage = "Veuillez définir les 2 premiers choix au minimum"),
            Display(Name = "Choix 2")]
        public string Choice_2 { get; set; }
        [Display(Name = "Choix 3")]
        public string Choice_3 { get; set; }
        [Display(Name = "Choix 4")]
        public string Choice_4 { get; set; }
        [Display(Name = "Choix 5")]
        public string Choice_5 { get; set; }
        #endregion

        public int CurrentUserId { get; set; }

        public List<string> Choices { get; set; }
    }
}
