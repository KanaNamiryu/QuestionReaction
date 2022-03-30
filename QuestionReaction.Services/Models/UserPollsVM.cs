using QuestionReaction.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionReaction.Services.Models
{
    public class UserPollsVM
    {
        public List<Question> CreatedPolls { get; set; }
        public List<Question> JoinedPolls { get; set; }

    }
}
