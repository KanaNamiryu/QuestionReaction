using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QuestionReaction.Services.Interfaces;
using QuestionReaction.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QuestionReaction.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;

        public HomeController(ILogger<HomeController> logger, ILoginService loginService, IRegisterService registerService)
        {
            _logger = logger;
            _loginService = loginService;
            _registerService = registerService;
        }

        /// <summary>
        /// Page d'accueil du site
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Page d'affichage des privacy
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Page de connexion
        /// </summary>
        /// <param name="returnUrl">URL vers lequel l'utilisateur sera redirigé une fois connecté</param>
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            if (User.Claims.FirstOrDefault() != null) // si déja connecté
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new LoginVM();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Connexion de l'utilisateur si il à les bon acces
        /// </summary>
        /// <param name="model">Model contenant les entrée de l'utilisateur</param>
        /// <returns>Redirige l'utilisateur vers l'URL demandé, sinon le redirige vers la liste des sondages</returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                bool isConnected = await _loginService.LoginAsync(model.Login, model.Password, model.RememberMe);
                if (isConnected)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Polls", "User");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Identifiant ou mot de passe invalide");
                    return View(model);
                }
            }
        }

        /// <summary>
        /// Deconnexion de l'utilisateur
        /// </summary>
        /// <returns>Redirige l'utilisateur vers la page d'accueil</returns>
        public async Task<IActionResult> Logout()
        {
            await _loginService.LogoutAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Page d'inscription de l'utilisateur
        /// </summary>
        /// <param name="returnUrl">URL vers lequel l'utilisateur sera redirigé une fois inscrit</param>
        [HttpGet]
        public IActionResult Registration(string returnUrl)
        {
            if (User.Claims.FirstOrDefault() != null) // si déja connecté
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new RegisterVM();
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        /// <summary>
        /// Inscrit l'utilisateur si les informations qu'il a entrées sont correctes
        /// </summary>
        /// <param name="model">Model contenant les informations entrées par l'utilisateur</param>
        /// <returns>Redirige l'utilisateur vers l'URL demandé, sinon le redirige vers la page de connexion</returns>
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            else
            {
                int isRegister = await _registerService.RegisterAsync(model.Name, model.Mail, model.Login, model.Password);
                if (isRegister == 0) // si pas d'erreur a l'inscription
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Login));
                    }
                }
                else
                {
                    switch (isRegister)
                    {
                        case 1:
                            ModelState.AddModelError("", "Le mot de passe doit contenir au moins une majuscule, une minuscule, un chiffre et un caractère spécial");
                            break;
                        case 2:
                            ModelState.AddModelError("", "Cet identifiant est déjà utilisé");
                            break;
                        case 3:
                            ModelState.AddModelError("", "Cette adresse mail est déjà utilisée");
                            break;
                        default:
                            ModelState.AddModelError("", "Une erreur inconnue s'est produite");
                            break;
                    }
                    return View(model);
                }
            }
        }

        /// <summary>
        /// Page d'erreur
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
