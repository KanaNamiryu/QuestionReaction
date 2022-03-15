using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace QuestionReaction.Web.Controllers
{
    public class UserController: Controller
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        public IActionResult Questions()
        {
            return View();
        }

    }
}
