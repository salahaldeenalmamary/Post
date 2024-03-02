using System.ComponentModel.DataAnnotations;

namespace Comment_Post.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        //[Required]
        //  [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 5)]
        public string Title { get; set; }

       // [Required]
     //   [StringLength(500, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 10)]
        public string Content { get; set; }
        public string? UserName { get; set; }
    }
}
