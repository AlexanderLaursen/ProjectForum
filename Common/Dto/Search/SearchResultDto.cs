﻿using Common.Enums;

namespace Common.Dto.Search
{
    public class SearchResultDto
    {
        public int Id { get; set; }
        public ContentType Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}