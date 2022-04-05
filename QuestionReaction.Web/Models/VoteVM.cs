using QuestionReaction.Data.Model;

namespace QuestionReaction.Web.Models
{
    public class VoteVM
    {
        public Question Question { get; set; }
        public int ChoicesQuantity => Question.Choices.Count;
        public int VoteNumber { get; set; }

    }
}
