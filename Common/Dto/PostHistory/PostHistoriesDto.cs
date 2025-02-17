using Common.Dto.Post;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto.PostHistory
{
    public class PostHistoriesDto
    {
        public List<PostHistoryDto> PostHistories { get; set; } = new List<PostHistoryDto>();
        public PostDto Post { get; set; } = new PostDto();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}
