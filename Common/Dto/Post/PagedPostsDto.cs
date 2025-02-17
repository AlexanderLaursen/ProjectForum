using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto.Post
{
    public class PagedPostsDto
    {
        public List<PostDto> Posts { get; set; } = new List<PostDto>();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}
