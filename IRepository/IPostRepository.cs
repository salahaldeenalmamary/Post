using Comment_Post.Models;
using Comment_Post.ViewModels;

namespace Comment_Post.IRepository
{
    public interface IPostRepository
    {
        Task<List<PostViewModel>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task AddPostAsync(PostViewModel post, string userId);
        Task UpdatePostAsync(PostViewModel post, string userId);
        Task DeletePostAsync(Post post);
        bool PostExists(int id);
     
    }
}
