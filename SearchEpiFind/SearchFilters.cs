using System.Collections.Generic;
using System.Web;

namespace SearchEpiFind.Business.Search
{
    public class SearchFilters : Dictionary<string, List<string>>
    {

        /// <summary>
        /// Covert dictionary to string representation
        /// to pass to query string
        /// </summary>
        /// <returns></returns>
        public string ToQueryString()
        {
            var filters = new List<string>();

            foreach (var filter in this)
            {
                filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(filter.Key),
                        HttpUtility.UrlEncode(string.Join(",", string.Join(",", filter.Value)))));
            }

            return string.Join(";", filters);
        }
    }
}