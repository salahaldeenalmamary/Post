using Comment_Post.Models;
using Comment_Post.ViewModels;

namespace Comment_Post.IRepository
{



    public interface IAuthorizationRepository
        {
            Task<IEnumerable<string>> GetUserRolesAsync(AppUser user);
            Task<bool> AddUserToRoleAsync(AppUser user, string roleName);
            Task<bool> RemoveUserFromRoleAsync(AppUser user, string roleName);
        Task<ResultViewModel<AppUser>> LoginAsync(LoginViewModel model );
            Task<ResultViewModel<AppUser>> RegisterAsync(RegisterViewModel model);
        }
    

}
