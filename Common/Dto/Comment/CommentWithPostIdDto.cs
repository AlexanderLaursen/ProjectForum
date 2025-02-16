﻿using Common.Dto.User;

namespace Common.Dto.Comment
{
    public class CommentWithPostIdDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public int PostId { get; set; }

        public string UserId { get; set; }
        public ShortUserDto User { get; set; }
    }
}