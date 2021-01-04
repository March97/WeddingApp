namespace WeddingApp.API.Helpers
{
    public class PlaceParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        public int PlaceId { get; set; }

        public string City { get; set; }

        public int MinPrice { get; set; } = 0;

        public int MaxPrice { get; set; } = 100000;

        public int MinCapacity { get; set; } = 0;

        public int MaxCapacity { get; set; } = 100000;
        
        public string OrderBy { get; set; }
    }
}