using QuestionReaction.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Web.Models
{
    public class UserPollsVM
    {
        public List<QuestionsVM> CreatedPolls { get; set; }
        public List<QuestionsVM> JoinedPolls { get; set; }
        public string VoteUid { get; set; }

    }

    public struct QuestionsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool MultipleChoices { get; set; }
        public string VoteUid { get; set; }
        public string ResultUid { get; set; }
        public bool IsActive { get; set; }

    }
}
