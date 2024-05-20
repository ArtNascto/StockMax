using StockMax.Domain.Models.View.Helpers;

namespace StockMax.API.Utils
{
    public static class QueryUtil<T>
    {
        public static dynamic ExpandSingleItem(T item)
        {
            var resourceToReturn = item.ToDynamic() as IDictionary<string, object>;

            return resourceToReturn;
        }
    }
}