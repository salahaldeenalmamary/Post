
using Comment_Post.Repository.Comment_Post.Repositories;
using Comment_Post.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Comment_Post.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUsersWithPosts()
        {
            var usersWithPostsEF = _userRepository.GetUsersWithPostsLast30Days();
            var usersWithPostsDapper = _userRepository.GetUsersWithPostsLast30DaysDapper();

            var result = new
            {
                usersWithPostsEF,
                usersWithPostsDapper
            };

            return Json(result);
        }
        public IActionResult UpdateProfile()
        {
            // Fetch user profile data or create a ViewModel
            var userProfileViewModel = new UserProfileViewModel
            {
                Name = "John Doe", 
                Password = "*****", 
                Address = "123 Main St" 
            };

            // Return the partial view for updating the user profile
            return PartialView("_UserProfilePartial", userProfileViewModel);
        }

    }
}
