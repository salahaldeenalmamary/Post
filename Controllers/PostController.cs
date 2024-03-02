
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Comment_Post.Models;
using Comment_Post.Repository;
using System.Threading.Tasks;
using Comment_Post.IRepository;
using Comment_Post.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Comment_Post.Constant;

namespace Comment_Post.Controllers
{
    [Authorize(Roles = "admin")]

    public class PostController : Controller
        {
            private readonly IPostRepository _postRepository;

            public PostController(IPostRepository postRepository)
            {
                _postRepository = postRepository;
            }

           
            public async Task<IActionResult> Index()
            {
               var posts = await _postRepository.GetAllPostsAsync();
                return View(posts);
            }

       
       

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllPostsAsync();
            return Json(posts);
        }

        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (id == null)
            {
                // Create new post
                var newPostViewModel = new PostViewModel();
                return PartialView("_PostForm", newPostViewModel);
            }
            else
            {
               
                var post = await _postRepository.GetPostByIdAsync(id.Value);
                if (post == null)
                {
                    return NotFound();
                }
                var editPostViewModel = new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content
                };
                return PartialView("_PostForm", editPostViewModel);
            }
        }

       

       
            public async Task<IActionResult> Delete(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var post = await _postRepository.GetPostByIdAsync(id.Value);

                if (post == null)
                {
                    return NotFound();
                }

                return View(post);
            }

            // POST: Post/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var post = await _postRepository.GetPostByIdAsync(id);
                await _postRepository.DeletePostAsync(post);
                return RedirectToAction(nameof(Index));
            }
        [HttpGet]
        public async Task<IActionResult> AddEditPost(int? id)
        {
            if (id.HasValue)
            {
                // Editing an existing post
                var post = await _postRepository.GetPostByIdAsync(id.Value);
                return PartialView("AddEditPost", post);
            }
            else
            {
                // Adding a new post
                return PartialView("AddEditPost", new Post());
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(PostViewModel post)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

               
                if (post.Id == 0)
                {
                    

                    await _postRepository.AddPostAsync(post, userId);
                }
                else
                {
                   
                   

                    await _postRepository.UpdatePostAsync(post,userId);
                }

                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }


    }
}


