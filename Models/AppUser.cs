using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace Comment_Post.Models
{
    public class AppUser : IdentityUser
    {
        public string? name { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Post> Posts { get; set; }
        public ICollection<Comment> Comments { get; set; }

        
    }

}
