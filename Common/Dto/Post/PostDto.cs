﻿using Common.Dto.Comment;
using Common.Dto.User;

namespace Common.Dto.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public int ViewCount { get; set; }
        public bool Edited { get; set; }
        public bool LikedByUser { get; set; }
        public int CommentsCount { get; set; }

        public int CategoryId { get; set; }

        public ShortUserDto User { get; set; }

        public List<CommentDto> Comments { get; set; }
    }
}