﻿using System.ComponentModel.DataAnnotations;

namespace Comment_Post.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

      
        public int PostId { get; set; }
        public Post Post { get; set; }
    }

}
