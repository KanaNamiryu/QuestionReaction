using System.Collections.Generic;

namespace QuestionReaction.Web.Models
{
    public class PollsLinksPageVM
    {
        public string VoteLink { get; set; }
        public string ResultLink { get; set; }
        public string DisableLink { get; set; }
        public string GuestsMailsString { get; set; }
        public List<string> GuestsMails { get; set; }
    }
}
