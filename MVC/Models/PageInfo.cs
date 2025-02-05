namespace MVC.Models
{
    public class PageInfo
    {
        private const int DEFAULT_PAGE_SIZE = 10;

        private int _currentPage;
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = ValidatePage(value);
            }
        }

        private int _pageSize = DEFAULT_PAGE_SIZE;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                _pageSize = ValidatePageSize(DEFAULT_PAGE_SIZE);
            }
        }

        private int _totalItems;
        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = ValidateTotalItems(value);
            }
        }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public int Skip => (CurrentPage - 1) * PageSize;

        public PageInfo()
        {
        }

        public PageInfo(int currentPage)
        {
            CurrentPage = currentPage;
        }

        public PageInfo(int currentPage, int itemsPerPage)
        {
            CurrentPage = currentPage;
            PageSize = itemsPerPage;
        }

        public PageInfo(int currentPage, int itemsPerPage, int totalItems)
        {
            CurrentPage = currentPage;
            PageSize = itemsPerPage;
            TotalItems = totalItems;
        }

        private static int ValidatePage(int page)
        {
            return page <= 0 ? 1 : page;
        }

        private static int ValidatePageSize(int pageSize)
        {
            return pageSize <= 0 ? DEFAULT_PAGE_SIZE : pageSize;
        }

        private static int ValidateTotalItems(int totalItems)
        {
            return totalItems < 0 ? 0 : totalItems;
        }
    }
}
