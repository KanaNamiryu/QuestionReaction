using System.ComponentModel.DataAnnotations;

namespace QuestionReaction.Web.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Identifiant obligatoire")]
        [Display(Name = "Identifiant")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Mot de passe obligatoire")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum 8 caractère")]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }
        [Display(Name = "Rester connecté")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

    }
}
