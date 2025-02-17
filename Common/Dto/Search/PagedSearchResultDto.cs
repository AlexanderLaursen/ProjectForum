using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dto.Search
{
    public class PagedSearchResultDto
    {
        public List<SearchResultDto> SearchResults { get; set; } = new List<SearchResultDto>();
        public PageInfo PageInfo { get; set; } = new PageInfo();
    }
}
