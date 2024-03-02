using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comment_Post.Data;
using Comment_Post.IRepository;
using Comment_Post.Models;
using Comment_Post.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Comment_Post.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<PostViewModel>> GetAllPostsAsync()
        {
            return await _context.posts
                .Include(p => p.User)
                .Select(post => new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    UserName = post.User != null ? post.User.UserName : null,
           
        })
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _context.posts.Include(p => p.User).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddPostAsync(PostViewModel postViewModel, string userId)
        {
            var post = MapToPost(postViewModel, userId);
          
            _context.posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePostAsync(PostViewModel postViewModel, string userId)
        {
            var post = MapToPost(postViewModel, userId, isUpdate: true);

          var  exitpost =   await  GetPostByIdAsync(post.Id);

            exitpost.UpdatedAt = post.UpdatedAt;
            exitpost.Title = post.Title;
            exitpost.Content = post.Content;

            _context.posts.Update(exitpost);
          
            await _context.SaveChangesAsync();
        }



        private Post MapToPost(PostViewModel postViewModel, string userId, bool isUpdate = false)
        {
            var post = new Post
            {
                
                Title = postViewModel.Title,
                Content = postViewModel.Content,
                UserId = userId
            };

            if (!isUpdate)
            {
                post.CreatedAt = DateTime.Now;
            }
            else
            {
                post.UpdatedAt = DateTime.Now;
                post.Id = postViewModel.Id;
            }

            return post;
        }


        public async Task DeletePostAsync(Post post)
        {
            _context.posts.Remove(post);
            await _context.SaveChangesAsync();
        }

        public  bool PostExists(int id)
        {
            return  _context.posts.Any(p => p.Id == id);
        }

      
    }
}
