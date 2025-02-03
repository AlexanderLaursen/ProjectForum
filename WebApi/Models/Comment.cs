﻿namespace WebApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }


        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
