namespace WebApi.Models
{
    public class PageInfo
    {
        private const int DEFAULT_PAGE_SIZE = 10;

        private int _currentPage = 1;
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
                _pageSize = ValidatePageSize(value);
            }
        }

        private int _totalItems = 0;
        public int TotalItems
        {
            get => _totalItems;
            set
            {
                _totalItems = ValidateTotalItems(value);
            }
        }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);

        public int Skip { get; private set; }

        public PageInfo()
        {
            Skip = (CurrentPage - 1) * PageSize;
        }

        public PageInfo(int currentPage)
        {
            CurrentPage = currentPage;
            Skip = (CurrentPage - 1) * PageSize;
        }

        public PageInfo(int currentPage, int itemsPerPage)
        {
            CurrentPage = currentPage;
            PageSize = itemsPerPage;
            Skip = (CurrentPage - 1) * PageSize;
        }

        public PageInfo(int currentPage, int itemsPerPage, int totalItems)
        {
            CurrentPage = currentPage;
            PageSize = itemsPerPage;
            TotalItems = totalItems;
            Skip = (CurrentPage - 1) * PageSize;
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
