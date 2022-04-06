using QuestionReaction.Data.Model;

namespace QuestionReaction.Web.Models
{
    public class VoteVM
    {
        public Question Question { get; set; }
        public int QuestionId { get; set; }
        public int VoteNumber { get; set; }
        public int[] SelectedChoices { get; set; }

    }
}
