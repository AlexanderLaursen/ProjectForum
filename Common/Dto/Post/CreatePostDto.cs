﻿namespace Common.Dto.Post
{
    public class CreatePostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int CategoryId { get; set; }
    }
}