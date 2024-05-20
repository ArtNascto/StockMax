using Newtonsoft.Json;

namespace StockMax.Domain.Models.View
{
    public class QueryParameters
    {
        private const int maxPageCount = 50;

        [JsonProperty("page")]
        public int Page { get; set; } = 1;

        private int _pageCount = maxPageCount;

        [JsonProperty("pageCount")]
        public int PageCount
        {
            get { return _pageCount; }
            set { _pageCount = (value > maxPageCount) ? maxPageCount : value; }
        }

        [JsonProperty("query")]
        public string? Query { get; set; }

        [JsonProperty("status")]
        public string? Status { get; set; }

        [JsonProperty("orderBy")]
        public string OrderBy { get; set; } = "Code";
    }
}