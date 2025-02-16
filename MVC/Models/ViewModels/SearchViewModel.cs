using Common.Dto;

namespace MVC.Models.ViewModels
{
    public class SearchViewModel
    {
        public List<SearchResultDto> SearchResults { get; set; }
        public Common.Models.PageInfo PageInfo { get; set; }
        public string SearchString { get; set; }

        public SearchViewModel()
        {
            SearchResults = [];
            PageInfo = new();
            SearchString = string.Empty;
        }
        
        public SearchViewModel(string searchString)
        {
            SearchResults = [];
            PageInfo = new();
            SearchString = searchString;
        }
    }
}