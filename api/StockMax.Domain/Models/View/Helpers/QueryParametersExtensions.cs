namespace StockMax.Domain.Models.View.Helpers
{
    public static class QueryParametersExtensions
    {
        public static bool HasPrevious(this QueryParameters queryParameters)
        {
            return queryParameters.Page > 1;
        }

        public static bool HasNext(this QueryParameters queryParameters, int totalCount)
        {
            return queryParameters.Page < (int)GetTotalPages(queryParameters, totalCount);
        }

        public static double GetTotalPages(this QueryParameters queryParameters, int totalCount)
        {
            return Math.Ceiling(totalCount / (double)queryParameters.PageCount);
        }

        public static bool HasQuery(this QueryParameters queryParameters)
        {
            return !string.IsNullOrEmpty(queryParameters.Query) && queryParameters.Query != "null" && queryParameters.Query != "undefined";
        }

        public static bool IsDescending(this QueryParameters queryParameters)
        {
            if (!string.IsNullOrEmpty(queryParameters.OrderBy))
            {
                return queryParameters.OrderBy.Split(' ').Last().ToLower().StartsWith("desc");
            }
            return false;
        }
    }
}