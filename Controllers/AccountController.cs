using Microsoft.AspNetCore.Mvc;
using Comment_Post.Models;
using System.Threading.Tasks;
using Comment_Post.IRepository;
using Comment_Post.ViewModels;
using Comment_Post.middleware;
using Microsoft.AspNetCore.SignalR;

namespace Comment_Post.Controllers
{
  

    public class AccountController : Controller
    {
        private readonly IAuthorizationRepository _authorizationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        public AccountController(IAuthorizationRepository authorizationRepository, IHubContext<NotificationHub> hubContext)
        {
            _authorizationRepository = authorizationRepository;
            _hubContext = hubContext;
        }

        
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authorizationRepository.LoginAsync(model);

                if (result.IsSuccess)
                {
                    // Successful login
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    
                    ModelState.AddModelError(string.Empty, "Invalid login attempt");
                }
            }

            // Model is not valid or login failed
            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var registrationResult = await _authorizationRepository.RegisterAsync(model);

                if (registrationResult.IsSuccess)
                {
                    // Registration succeeded
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Registration failed, handle errors
                    foreach (var error in registrationResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                    return View(model);
                }

            }

            // Model is not valid or registration failed
            return View(model);
        }
    }

}
