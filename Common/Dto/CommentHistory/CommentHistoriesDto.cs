using Common.Dto.Comment;
using Common.Models;

namespace Common.Dto.CommentHistory
{
    public class CommentHistoriesDto
    {
        public List<CommentHistoryDto> CommentHistories { get; set; }
        public CommentDto Comment { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}
