using Microsoft.EntityFrameworkCore;
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
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly IPollService _pollService;
        private readonly AppDbContext _ctx;
        private readonly IUserService _userService;
        private int CurrentUserId => int.Parse(User.Claims.Single(u => u.Type == "id").Value);
        public UserController(ILogger<UserController> logger, IPollService pollService, AppDbContext ctx, IUserService userService)
        {
            _logger = logger;
            _pollService = pollService;
            _ctx = ctx;
            _userService = userService;
        }

        /// <summary>
        /// Page d'affichage de la liste des sondages créés et rejoins
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Polls()
        {
            var model = new UserPollsVM();

            var user = await _userService.GetUserByIdAsync(CurrentUserId);

            // liste des sondages créés par l'utilisateur
            model.CreatedPolls = _pollService.GetQuestionsByUserIdAsync(CurrentUserId)
                .Result
                .Select(p => new QuestionsVM()
                {
                    Id = p.Id,
                    Title = p.Title,
                    MultipleChoices = p.MultipleChoices,
                    VoteUid = p.VoteUid,
                    ResultUid = p.ResultUid,
                    IsActive = p.IsActive
                })
                .ToList();

            // liste des sondages auxquels l'utilisateur à été invité sauf ceux qu'il a créé
            var allPolls = await _pollService.GetQuestionsByGuestMailAsync(user.Mail);
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
                        ResultUid = q.ResultUid,
                        IsActive = q.IsActive
                    })
                    .ToList();
            }

            return View(model);
        }

        /// <summary>
        /// Voter à un sondage à partir de son uid ou lien
        /// </summary>
        /// <param name="model">Model contenant la string entrée par l'utilisateur</param>
        /// <returns>Redirige l'utilisateur vers la page de vote correspondante si il a entré une chaine de charactere valide</returns>
        [HttpPost]
        public async Task<IActionResult> Polls(UserPollsVM model)
        {
            var voteUid = " ";
            if (!string.IsNullOrEmpty(model.VoteUid))
            {
                voteUid = model.VoteUid;
            }
            var linkBase1 = "https://" + Request.Host.Value + "/User/Vote?voteUid=";
            var linkBase2 = Request.Host.Value + "/User/Vote?voteUid=";
            
            if (voteUid.StartsWith(linkBase1)) // commence par https....
            {
                voteUid = voteUid.Remove(0, linkBase1.Length);
            }

            if (voteUid.StartsWith(linkBase2)) // commence par ....
            {
                voteUid = voteUid.Remove(0, linkBase2.Length);
            }

            var uidExiste = await _pollService.VoteUidExistsAsync(voteUid);

            if (voteUid.Length == 32 && uidExiste) // uid à la bonne longueur ET existe dans la BDD
            {
                return RedirectToAction(nameof(Vote), new { voteUid });
            }
            else // string inconnue
            {
                return RedirectToAction(nameof(Polls));
            }
        }

        /// <summary>
        /// Page de création d'un nouveau sondage
        /// </summary>
        [HttpGet]
        public IActionResult AddPolls()
        {
            var model = new UserAddPollsVM() { CurrentUserId = CurrentUserId };
            return View(model);
        }

        /// <summary>
        /// Création d'un sondage d'apres les choix de l'utilisateur
        /// </summary>
        /// <param name="model">Model contenant les choix de l'utilisateur</param>
        /// <returns>Redirige vers la page de gestion du sondage</returns>
        [HttpPost]
        public async Task<IActionResult> AddPolls(UserAddPollsVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                model.Choices = model.Choices
                    .Where(c => c != null)
                    .ToList();
                var pollId = await _pollService.AddPollAsync(model);

                return RedirectToAction(nameof(PollsLinks), new { pollId });
            }
        }

        /// <summary>
        /// Page de gestion d'un sondage
        /// </summary>
        /// <param name="pollId">id du sondage</param>
        [HttpGet]
        public async Task<IActionResult> PollsLinks(int pollId)
        {
            var poll = await _pollService.GetQuestionByIdAsync(pollId);
            var linkBase = "https://" + Request.Host.Value + "/User/";
            var model = new PollsLinksPageVM()
            {
                VoteLink = linkBase + "Vote?voteUid=" + poll.VoteUid,
                VoteUid = poll.VoteUid,
                ResultLink = linkBase + "Result?resultUid=" + poll.ResultUid,
                ResultUid = poll.ResultUid,
                DisableLink = linkBase + "Disable?disableUid=" + poll.DisableUid,
                DisableUid = poll.DisableUid,
                IsActive = poll.IsActive,
                QuestionId = pollId
            };
            return View(model);
        }

        /// <summary>
        /// Désactivation d'un sondage par son uid de désactivation
        /// </summary>
        /// <param name="disableUid">Uid de désactivation du sondage</param>
        /// <returns>Redirige vers la page de liste des sondages</returns>
        [HttpGet]
        public async Task<IActionResult> Disable(string disableUid)
        {
            await _pollService.DisableQuestionAsync(disableUid);
            return RedirectToAction(nameof(Polls));
        }

        /// <summary>
        /// Page d'ajout de vote à un sondage par son uid de vote
        /// </summary>
        /// <param name="voteUid">Uid de vote du sondage</param>
        /// <returns>Redirige l'utilisateur vers la page des résultats si il a déja voté
        /// Redirige l'utilisateur vers la page d'erreur spécifique si il n'a pas été invité</returns>
        [HttpGet]
        public async Task<IActionResult> Vote(string voteUid)
        {
            var question = await _pollService.GetQuestionByVoteUidAsync(voteUid);
            var user = await _userService.GetUserByIdAsync(CurrentUserId);

            var alreadyVoted = await _pollService.AsAlreadyVotedAsync(CurrentUserId, question.Id);

            if (!question.IsActive || alreadyVoted) // si sondage desactivé OU utilisateur à deja voté → redirection vers les resultats
            {
                return RedirectToAction(nameof(Result), new { resultUid = question.ResultUid });
            }
            else
            {
                if (await _ctx.Guests
                   .Where(g => g.Mail == user.Mail)
                   .Where(g => g.Question == question)
                   .SingleAsync() == null) // le mail de l'utilisateur n'est pas dans la liste des invités
                {
                    return RedirectToAction(nameof(ErrorNotInvited));
                }
                else // l'utilisateur est invité et peut donc voter
                {
                    var model = new VoteVM()
                    {
                        Question = question,
                        VoteNumber = question.Reactions
                            .Where(r => r.QuestionId == question.Id)
                            .Count()
                    };
                    return View(model);
                }
            }
        }

        /// <summary>
        /// Ajout de vote à un sondage d'apres les choix de l'utilisateur
        /// </summary>
        /// <param name="model">Model contenant les choix de l'utilisateur</param>
        /// <returns>Redirige vers la page de résultat du sondage</returns>
        [HttpPost]
        public async Task<IActionResult> Vote(VoteVM model)
        {
            var resultUid = await _pollService.AddReactionsAsync(
                model.SelectedChoices.ToList(),
                CurrentUserId);
            return RedirectToAction(nameof(Result), new { resultUid });
        }

        /// <summary>
        /// Page d'affichage des résultats d'un sondage par son uid de résultat
        /// </summary>
        /// <param name="resultUid">Uid de résultat du sondage</param>
        [HttpGet]
        public async Task<IActionResult> Result(string resultUid)
        {
            var question = await _pollService.GetQuestionByResultUidAsync(resultUid);
            var model = new ResultVM()
            {
                Question = question,
                SortedChoices = await _pollService.SortChoicesByVoteNumberAsync(question.Id),
                VoteNumber = question.Reactions
                        .Where(r => r.QuestionId == question.Id)
                        .Count(),
                DistinctUserNumber = question.Reactions
                    .GroupBy(r => r.UserId)
                    .Select(g => g.First())
                    .ToList()
                    .Count
            };
            return View(model);
        }

        /// <summary>
        /// Page d'erreur indiquant à l'utilisateur qu'il n'est pas invité au sondage auquel il essaye de voter
        /// </summary>
        public IActionResult ErrorNotInvited()
        {
            return View();
        }


        public async Task<IActionResult> Invite(PollsLinksPageVM model)
        {
            var mailsString = model.GuestsMailsString;
            var mailsList = mailsString.Split(',').ToList();

            await _pollService.AddGuestsAsync(mailsList, model.QuestionId);

            return RedirectToAction(nameof(PollsLinks), new { pollId = model.QuestionId });
        }
    }
}
