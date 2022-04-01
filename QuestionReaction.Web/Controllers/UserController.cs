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

            // liste des sondages créés par l'utilisateur
            model.CreatedPolls = _ctx.Users
                .Select(u => u.Questions)
                .FirstOrDefault()
                .Select(p => new QuestionsVM()
                {
                    Id = p.Id,
                    Title = p.Title,
                    MultipleChoices = p.MultipleChoices,
                    VoteUid = p.VoteUid,
                    ResultUid = p.ResultUid
                })
                .ToList();

            var user = await _userService.GetUserByIdAsync(_currentUserId);

            // liste des sondages auxquels l'utilisateur à été invité sauf ceux qu'il a créé
            model.JoinedPolls = _ctx.Guests
                .Where(g => g.Mail == user.Mail)
                .ToList()
                .Select(g => g.Question)
                .ToList()
                .Where(q => q.User != user)
                .Select(q => new QuestionsVM()
                {
                    Id=q.Id,
                    Title=q.Title,
                    MultipleChoices=q.MultipleChoices,
                    VoteUid=q.VoteUid,
                    ResultUid=q.ResultUid
                })
                .ToList();

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
                await _pollService.AddPollAsync(model);
                return RedirectToAction(nameof(Polls)); // redirection a changer vers la page des liens
            }
        }

        public IActionResult PollsLinks()
        {
            var model = new PollsLinksPageVM();
            return View(model);
        }
    }
}
