using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionReaction.Data;
using QuestionReaction.Services.Interfaces;
using QuestionReaction.Services.Models;
using QuestionReaction.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionReaction.Web.Controllers
{
    [Authorize]
    public class UserController: Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPollService _pollService;
        private readonly AppDbContext _ctx;
        private readonly IUserService _userService;
        private int _currentUserId => int.Parse(User.Claims.Single(u => u.Type == "id").Value);
        public UserController(ILogger<UserController> logger, IPollService pollService, AppDbContext ctx, IUserService userService)
        {
            _logger = logger;
            _pollService = pollService;
            _ctx = ctx;
            _userService = userService;
        }

        public async Task<IActionResult> Polls()
        {
            var model = new UserPollsVM();

            var user = await _userService.GetUserByIdAsync(_currentUserId);

            // liste des sondages créés par l'utilisateur
            model.CreatedPolls = _pollService.GetQuestionsByUserIdAsync(_currentUserId)
                .Result
                .Select(p => new QuestionsVM()
                {
                    Id = p.Id,
                    Title = p.Title,
                    MultipleChoices = p.MultipleChoices,
                    VoteUid = p.VoteUid,
                    ResultUid = p.ResultUid
                })
                .ToList();

            // liste des sondages auxquels l'utilisateur à été invité sauf ceux qu'il a créé
            var allPolls = await _pollService.GetQuestionsByGuestAsync(user.Mail);
            if (allPolls != null)
            {
                model.JoinedPolls = allPolls
                    .Where(q => q.User != user)
                    .Select(q => new QuestionsVM()
                    {
                        Id = q.Id,
                        Title = q.Title,
                        MultipleChoices = q.MultipleChoices,
                        VoteUid = q.VoteUid,
                        ResultUid = q.ResultUid
                    })
                    .ToList();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AddPolls()
        {
            var model = new UserAddPollsVM() { CurrentUserId = _currentUserId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddPolls(UserAddPollsVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                model.Choices = new List<string>
                    {
                        model.Choice1, model.Choice2, model.Choice3, model.Choice4, model.Choice5
                    }
                    .Where(c => c != null)
                    .ToList();
                var pollId = await _pollService.AddPollAsync(model);

                return RedirectToAction(nameof(PollsLinks), new { pollId = pollId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PollsLinks(int pollId)
        {
            //Request.Host.Value
            var poll = await _pollService.GetQuestionByIdAsync(pollId);
            var linkBase = "https://" + Request.Host.Value + "/User/";
            var model = new PollsLinksPageVM()
            {
                VoteLink = linkBase + "Vote?voteUid=" + poll.VoteUid,
                ResultLink = linkBase + "Result?resultUid=" + poll.ResultUid,
                DisableLink = linkBase + "Disable?disableUid=" + poll.DisableUid
            };
            return View(model);
        }

        public async Task<IActionResult> Disable(string disableUid)
        {
            await _pollService.DisableQuestionAsync(disableUid); // a corriger dans l'html pour recup uid a la place du link
            return RedirectToAction(nameof(Polls));
        }

        public IActionResult Vote(string voteUid)
        {
            return View();
        }

        public IActionResult Result(string resultUid)
        {
            return View();
        }
    }
}
