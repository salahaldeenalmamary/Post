using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Comment_Post.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

       
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public List<Comment> Comments { get; set; }
    }

}