using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto.Comment
{
    public class PagedCommentsDto
    {
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}
