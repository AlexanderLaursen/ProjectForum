using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Likes { get; set; }
        public int PostId { get; set; }
        public bool Edited { get; set; }
        public bool LikedByUser { get; set; }

        public string UserId { get; set; }
        public ShortUserDto User { get; set; }
    }
}
