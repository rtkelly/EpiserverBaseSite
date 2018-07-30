using SearchEpiFind.Business.Search.Util;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEpiFind.Business.Search
{
    public class SearchRequest
    {
        public string QueryText { get; set; }

        public string SearchWithinText { get; set; }
        
        public string Sort { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int Start { get; set; }

        public string RawFilterStr { get; set; }

        public bool SearchWithin { get; set; }
                
        public string From { get; set; }

        public string To { get; set; }

        public string AlwaysApplyFilter { get; set; }

        public SearchFilters CurrentFilters { get; set; }
        
        /// <summary>
        /// Intialize search request from query string
        /// </summary>
        /// <returns></returns>
        public static SearchRequest LoadSearchRequest(string defaultSort = null)
        {
            int page;

            int.TryParse(HttpContext.Current.Request["p"] ?? "1", out page);
            if (page == 0) page = 1;

            int pageSize;

            int.TryParse(HttpContext.Current.Request["n"] ?? "10", out pageSize);
            if (pageSize == 0) pageSize = 10;

            var request = new SearchRequest()
            {
                QueryText = HttpContext.Current.Request["q"] ?? "",
                SearchWithinText = HttpContext.Current.Request["f"] ?? "",
                RawFilterStr = HttpContext.Current.Request["r"] ?? "",
                Sort = HttpContext.Current.Request["s"]?.ToLower(),
                Page = page,
                PageSize = pageSize,
                SearchWithin = HttpContext.Current.Request["sw"] == "on",
                From = HttpContext.Current.Request["from"],
                To = HttpContext.Current.Request["to"],
                CurrentFilters = UnifiedSearchBuilder.CurrentFiltersFromString(HttpContext.Current.Request["r"] ?? ""),
            };

            request.Start = (request.Page - 1) * request.PageSize;

            if (!request.SearchWithin)
            {
                if (!string.IsNullOrEmpty(request.SearchWithinText))
                {
                    request.QueryText = request.SearchWithinText;
                    request.SearchWithinText = "";
                }
            }

            if (!string.IsNullOrEmpty(HttpContext.Current.Request["catid"]))
            {
                var categoryPath = CategoryPath.CreateCategoryPath(CategoryPath.GetCategoryRoot(), HttpContext.Current.Request["catid"]);

                if (categoryPath.Any())
                {
                    var c = string.Join("~", categoryPath);

                    if (request.CurrentFilters.ContainsKey("SearchCategories"))
                    {
                        request.CurrentFilters["SearchCategories"].Add(c);
                    }
                    else
                    {
                        request.CurrentFilters.Add("SearchCategories",
                        new List<string>() { c });
                    }
                }
            }
            
            if(string.IsNullOrEmpty(request.Sort) && defaultSort != null)
            {
                request.Sort = defaultSort.ToLower();
            }

            return request;
        }
    }
}