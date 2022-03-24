using System.ComponentModel.DataAnnotations;

namespace QuestionReaction.Web.Models
{
    public class RegisterVM
    {
        [Display(Name = "Nom"),
            MaxLength(100),
            MinLength(3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail obligatoire"),
            Display(Name = "E-mail"),
            DataType(DataType.EmailAddress),
            MaxLength(100)]
        public string Mail { get; set; }

        [Compare(nameof(Mail),ErrorMessage = "Les adresses e-mails doivent être identiques"),
            Display(Name = "Confirmer l'adresse e-mail")]
        public string CheckMail { get; set; }

        [Required(ErrorMessage = "Identifiant obligatoire"), 
            Display(Name = "Identifiant"),
            MaxLength(50),
            MinLength(3)]
        public string Login { get; set; }

        [Required(ErrorMessage = "Mot de passe obligatoire"), 
            Display(Name = "Mot de passe"),
            DataType(DataType.Password),
            MinLength(8, ErrorMessage = "Minimum 8 caractère")]
        public string Password { get; set; }

        [Compare(nameof (Password),ErrorMessage = "Les mots de passe doivent être identiques"),
            Display(Name = "Confirmer le mot de passe")]
        public string CheckPassword { get; set; }
        public string ReturnUrl { get; set; }

    }
}
