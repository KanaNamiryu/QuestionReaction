using QuestionReaction.Data.Model;

namespace QuestionReaction.Web.Models
{
    public class VoteVM
    {
        public Question Question { get; set; }
        public int ChoicesQuantity { get; set; }
        public int VoteNumber { get; set; }
        public string[] SelectedChoices { get; set; }

    }
}
