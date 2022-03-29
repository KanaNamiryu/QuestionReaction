using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionReaction.Data.Model;
using QuestionReaction.Services.Models;
using System.Collections.Generic;

namespace QuestionReaction.Web.Controllers
{
    public class UserController: Controller
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

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

        public IActionResult AddPolls(string returnUrl)
        {
            var model = new UserAddPollsVM();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

    }
}
