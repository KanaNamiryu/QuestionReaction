using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Interfaces;
using QuestionReaction.Services.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionReaction.Web.Controllers
{
    public class UserController: Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPollService _pollService;
        public UserController(ILogger<UserController> logger, IPollService pollService)
        {
            _logger = logger;
            _pollService = pollService;
        }

        [Authorize]
        public IActionResult Polls()
        {
            var model = new UserPollsVM()
            {
                CreatedPolls = new List<Question>()
                {
                    new Question()
                    {
                        Title = "quest 1"
                    },
                    new Question()
                    {
                        Title = "quest 2"
                    }
                },
                JoinedPolls = new List<Question>()
                {
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 3"
                    },
                    new Question()
                    {
                        Title = "quest 4"
                    }
                }
            };
            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddPolls()
        {
            var model = new UserAddPollsVM();
            model.CurrentUserId = int.Parse(User.Claims.Single(u => u.Type == "id").Value);
            return View(model);
        }

        [Authorize]
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

    }
}
