using QuestionReaction.Data.Model;
using System.Collections.Generic;

namespace QuestionReaction.Web.Models
{
    public class ResultVM
    {
        public Question Question { get; set; }
        public List<Choice> SortedChoices { get; set; }
        public int VoteNumber { get; set; }

    }
}
