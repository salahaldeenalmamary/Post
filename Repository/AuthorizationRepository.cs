namespace Comment_Post.Repository
{
  
    using global::Comment_Post.IRepository;
    using global::Comment_Post.Models;
    using global::Comment_Post.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;
    using System.Threading.Tasks;





    public class AuthorizationRepository : IAuthorizationRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthorizationRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> AddUserToRoleAsync(AppUser user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<bool> RemoveUserFromRoleAsync(AppUser user, string roleName)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            return result.Succeeded;
        }

        public async Task<ResultViewModel<AppUser>> LoginAsync(LoginViewModel model)
        {
            var result = new ResultViewModel<AppUser>();

            var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

            result.IsSuccess = signInResult.Succeeded;

            if (!signInResult.Succeeded)
            {
                result.Errors = new List<string> { "Invalid email or password" };
            }

            return result;
        }


        public async Task<ResultViewModel<AppUser>> RegisterAsync(RegisterViewModel model)
        {
            var result = new ResultViewModel<AppUser>();

            // Check if the user with the given email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                result.Errors.Add($"User with email '{model.Email}' already exists.");
                return result;
            }

           
            var user = new AppUser { Email = model.Email, UserName = model.Email, CreatedAt = DateTime.Now
        };
            var createResult = await _userManager.CreateAsync(user, model.Password);

            if (createResult.Succeeded)
            {
               
                var roleResult = await _userManager.AddToRoleAsync(user, "user");

                if (!roleResult.Succeeded)
                {
                    foreach (var error in roleResult.Errors)
                    {
                        result.Errors.Add($"Error assigning role: {error.Description}");
                    }
                    return result;
                }

                // Automatically sign in the user after successful registration
                await _signInManager.SignInAsync(user, isPersistent: false);

                result.IsSuccess = true;
                result.Data = user;
            }
            else
            {
               
                foreach (var error in createResult.Errors)
                {
                    result.Errors.Add($"Error: {error.Description}");
                }
            }

            return result;
        }




    }
}




