﻿using WebApi.Dto.Comment;

namespace WebApi.Dto.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int Likes { get; set; }
        public bool Edited { get; set; }

        public int CategoryId { get; set; }

        public ShortUserDto User { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}