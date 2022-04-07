using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAddPollsVM
    {
        /// <summary>
        /// Titre du sondage
        /// </summary>
        [Required(ErrorMessage = "Veuillez choisir un titre"),
            Display(Name = "Titre"),
            MaxLength(100)]
        public string Title { get; set; }
        /// <summary>
        /// Description du sondage
        /// </summary>
        [Display(Name = "Description"),
            MaxLength(300)]
        public string Description { get; set; }
        /// <summary>
        /// True = sondage en choix multiple
        /// </summary>
        [Display(Name = "Choix multiple")]
        public bool MutipleChoices { get; set; }
        /// <summary>
        /// Liste des choix entrée par l'utilisateur
        /// </summary>
        public List<string> Choices { get; set; }

        /// <summary>
        /// Id de l'utilisateur connecté
        /// </summary>
        public int CurrentUserId { get; set; }

    }
}
