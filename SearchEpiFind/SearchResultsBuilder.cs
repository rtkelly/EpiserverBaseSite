using EPiServer.Find.Api.Facets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.Find.UnifiedSearch;
using System.Text.RegularExpressions;
using SearchEpiFind.Business.Search.Util;

namespace SearchEpiFind.Business.Search
{
    public static class SearchResultsBuilder
    {
        /// <summary>
        /// Build list of facets from search results
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEnumerable<IMyFacet> FacetBuilder(this ISearchResultsViewModel model)
        {
            var searchPageFacets = model.Facets?.ToList() ?? new List<string>();

            if (model.WildCardFilters != null)
            {
                foreach (var filterOn in model.WildCardFilters)
                {
                    yield return new MyTermsFacet()
                    {
                        Name = filterOn.Key,
                        Terms = filterOn.Value
                                .Where(value => !string.IsNullOrEmpty(value))
                                .Select(value => value.Split('~'))
                                .Where(value => value.Count() == 2)
                                .Select(value => new MyTermCount()
                                {
                                    Title = value[1],
                                    Term = value[0],
                                    FieldName = filterOn.Key,
                                    Filter = model.BuildSingleSelectRefinement("filteron_" + filterOn.Key, value[0]),
                                    Selected = IsSelected(model.CurrentFilters, value[0]),
                                }),

                    };
                }
            }

            if (model.Results != null)
            {
                foreach (var facetResult in model.Results.Facets)
                {
                    if (facetResult is TermsFacet)
                    {
                        var termsFacet = facetResult as TermsFacet;

                        if (termsFacet.Name == "SearchCategories")
                        {
                            foreach (var parentTerm in termsFacet.Terms)
                            {
                                var cleanTerm = parentTerm.Term.GetTermFromPath();

                                if (model.AllCategoryFacets || searchPageFacets.Any(c => c == cleanTerm))
                                {
                                    var allSelected = model.ShowAllOptionInCategoryFacet && 
                                        model.CurrentFilters.ContainsKey("SearchCategories") &&
                                        model.CurrentFilters["SearchCategories"].Contains(parentTerm.Term);

                                    var terms = termsFacet.Terms.Select(t =>
                                                new { Path = t.Term.Trim(), Term = t.Term.GetTermFromPath(), t.Count })
                                            .Where(t => t.Path.StartsWith(parentTerm.Term + "~" + t.Term)
                                                        || (t.Path == parentTerm.Term &&
                                                        (model.AllTitleMap != null && model.AllTitleMap.ContainsKey(parentTerm.Term))))
                                            .Select(t => new MyTermCount()
                                            {
                                                Title = t.Path == parentTerm.Term && model.AllTitleMap.ContainsKey(parentTerm.Term) ? model.AllTitleMap[parentTerm.Term] : t.Term,
                                                Term = t.Path,
                                                Count = t.Count,
                                                Sort = t.Path == parentTerm.Term ? 0 : 1,
                                                FieldName = termsFacet.Name,
                                                Filter = t.Path == parentTerm.Term || allSelected ? BuildSingleSelectRefinement(model, termsFacet.Name, t.Path) : BuildRefinement(model, termsFacet.Name, t.Path),
                                                Selected = IsSelected(model.CurrentFilters, t.Path),
                                            }).ToList();

                                    if (!terms.Any())
                                        continue;

                                    yield return new MyTermsFacet()
                                    {
                                        Name = cleanTerm,
                                        Terms = model.SortFacetsByTitle ? terms.OrderBy(t => t.Title) : 
                                            terms.OrderBy(t => t.Sort).ThenByDescending(t => t.Count).ThenBy(t => t.Title),
                                    };
                                }

                            }
                        }
                        else
                        {

                            var myTermsFacet = new MyTermsFacet()
                            {
                                Name = model.FacetTitleMap != null && model.FacetTitleMap.ContainsKey(termsFacet.Name)
                                    ? model.FacetTitleMap[termsFacet.Name]
                                    : termsFacet.Name,
                                Terms = termsFacet.Terms
                                    //.Where(t => t.Count > 0)
                                    .Where(t => !string.IsNullOrEmpty(t.Term))
                                    .Select(t => new MyTermCount()
                                    {
                                        Title = t.Term,
                                        Term = t.Term,
                                        Count = t.Count,
                                        FieldName = termsFacet.Name,
                                        Filter = BuildRefinement(model, termsFacet.Name, t.Term),
                                        Selected = IsSelected(model.CurrentFilters, t.Term),
                                    }),
                            };

                            if (!myTermsFacet.Terms.Any())
                                continue;

                            yield return myTermsFacet;
                        }

                    }
                    else if (facetResult is DateRangeFacet)
                    {
                        var dateFacet = facetResult as DateRangeFacet;

                        var myDateFacet = new MyDateRangeFacet()
                        {
                            Name = model.FacetTitleMap != null && model.FacetTitleMap.ContainsKey(dateFacet.Name)
                                ? model.FacetTitleMap[dateFacet.Name]
                                : dateFacet.Name,

                            Terms = dateFacet.Ranges
                                .Where(t => t.Count > 0)
                                .Select(r => new MyDateRangeResult()
                                {
                                    Title = BuildRangeDateLabel(r.From, r.To),
                                    From = r.From,
                                    To = r.To,
                                    Count = r.Count,
                                    FieldName = dateFacet.Name,
                                    Filter = model.BuildDateRangeRefinement(dateFacet.Name, r.From, r.To),
                                    Selected = IsSelected(model.CurrentFilters, DateRangeToStr(r.From, r.To)),
                                }),
                        };

                        if (!myDateFacet.Terms.Any())
                            continue;

                        yield return myDateFacet;
                    }
                }
            }


        }


        /// <summary>
        /// Build facet query string parameters 
        /// using single select behavior
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        private static string BuildSingleSelectRefinement(this ISearchResultsViewModel model, string fieldName, string term)
        {
            // &r=SearchCategories[Format/Events, Format/Publications];SearchSection[Members Only]

            var filters = new List<string>();

            if (!model.CurrentFilters.Any(f => f.Key == fieldName))
            {
                filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(fieldName),
                    HttpUtility.UrlEncode(string.Join(",", new List<string>() { term }))));
            }

            foreach (var filter in model.CurrentFilters)
            {
                var values = new List<string>();
                
                if (fieldName == filter.Key)
                {
                    if (!filter.Value.Contains(term))
                        values.Add(term);

                }
                else if (filter.Value.Any())
                {
                    values.AddRange(filter.Value.Where(t => t != term));
                }

                if (values.Any())
                {
                    filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(filter.Key),
                        HttpUtility.UrlEncode(string.Join(",", values))));
                }
            }

            return string.Join(";", filters);
        }

        /// <summary>
        /// Build facet query string parameters
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static string BuildRefinement(this ISearchResultsViewModel model, string fieldName, string term)
        {
            // &r=SearchCategories[Format/Events, Format/Publications];SearchSection[Members Only]
                     
            var filters = new List<string>();

            if (!model.CurrentFilters.Any(f => f.Key == fieldName))
            {
                filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(fieldName),
                    string.Join(",", new List<string>() { HttpUtility.UrlEncode(term) })));
            }

            foreach (var filter in model.CurrentFilters)
            {
                var values = new List<string>();
                                
                if (fieldName == filter.Key)
                {
                    if (!filter.Value.Contains(term))
                        values.Add(term);
                }
                
                if(filter.Value.Any())
                {
                    values.AddRange(filter.Value.Where(t => t != term));
                }

                if (values.Any())
                {
                    filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(filter.Key),
                        HttpUtility.UrlEncode(string.Join(",", values))));
                }
            }

            return string.Join(";", filters);
        }

        /// <summary>
        /// Build Date Range facet query string parameters
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string BuildDateRangeRefinement(this ISearchResultsViewModel model, string fieldName, DateTime? start, DateTime? end)
        {
            // &r=SearchPublishDate[11/1/2017,12/1/2017];

            var filters = new List<string>();

            var dateRangeStr = DateRangeToStr(start, end);

            if (!model.CurrentFilters.Any(f => f.Key == fieldName && f.Value.Contains(dateRangeStr)))
                filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(fieldName), HttpUtility.UrlEncode(dateRangeStr)));
                        
            foreach (var filter in model.CurrentFilters)
            {
                if (fieldName == filter.Key)
                    continue;
                
                filters.Add(string.Format("{0}[{1}]", HttpUtility.UrlEncode(filter.Key),
                    HttpUtility.UrlEncode(string.Join(",", filter.Value))));
            }

            return string.Join(";", filters);
        }


        /// <summary>
        /// build facet refinement link href
        /// </summary>
        /// <param name="model"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static string BuildRefinementLink(this ISearchResultsViewModel model, IMyFacetItem term)
        {
            var queryStr = BuildQueryString(model, !string.IsNullOrEmpty(term.Filter) ? string.Format("r={0}", term.Filter) : "");

            return string.Format("{0}?{1}", model.PostBackUrl, queryStr);
        }

        /// <summary>
        /// build search page query string
        /// </summary>
        /// <param name="model"></param>
        /// <param name="queryString"></param>
        /// <param name="resetRange"></param>
        /// <returns></returns>
        public static string BuildQueryString(this ISearchResultsViewModel model, string queryString, bool resetRange=false)
        {
            var query = new List<string>
            {
                !string.IsNullOrEmpty(model.SrchRequest.QueryText) ? string.Format("q={0}", model.SrchRequest.QueryText) : "",
                !string.IsNullOrEmpty(model.SrchRequest.SearchWithinText) ? string.Format("f={0}", model.SrchRequest.SearchWithinText) : "",
                !string.IsNullOrEmpty(model.SrchRequest.Sort) ? string.Format("s={0}", model.SrchRequest.Sort) : ""
            };

            if (!resetRange)
            {
                query.Add(!string.IsNullOrEmpty(model.SrchRequest.From) ? string.Format("from={0}", model.SrchRequest.From) : "");
                query.Add(!string.IsNullOrEmpty(model.SrchRequest.To) ? string.Format("to={0}", model.SrchRequest.To) : "");
            }

            query.Add(model.SrchRequest.PageSize != 10 ? string.Format("n={0}", model.SrchRequest.PageSize) : "");
            query.Add(queryString);

            return string.Join("&", query.Where(str => str != ""));
        }

        /// <summary>
        /// Generate a list of pagination links
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Pagination> BuildPagination(this ISearchResultsViewModel model)
        {
            var totalPages = model.Results.TotalMatching / model.SrchRequest.PageSize;

            if (model.Results.TotalMatching % model.SrchRequest.PageSize != 0)
                totalPages++;

            var filters = HttpUtility.UrlEncode(model.SrchRequest.RawFilterStr);

            var queryString = model.BuildQueryString(string.Format("r={0}", filters));

            foreach (var page in Pagination.GetPagination(totalPages, model.SrchRequest.Page))
            {
                yield return new Pagination()
                {
                    Page = page,
                    Link = string.Format("{0}?{1}&p={2}", model.PostBackUrl, queryString, page),
                    Selected = model.SrchRequest.Page == page,
                };
            }
        }

        /// <summary>
        /// get total pages
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static int TotalPages(this ISearchResultsViewModel model)
        {
            if (model.Results == null)
            {
                return 0;
            }

            var totalPages = model.Results.TotalMatching / model.SrchRequest.PageSize;

            if (model.Results.TotalMatching % model.SrchRequest.PageSize != 0)
                totalPages++;

            return totalPages;

        }

        /// <summary>
        /// Generate first page link 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string FirstPage(this ISearchResultsViewModel model)
        {
            var filters = HttpUtility.UrlEncode(model.SrchRequest.RawFilterStr);

            return
                string.Format("{0}?{1}&p=1", model.PostBackUrl, 
                model.BuildQueryString(string.Format("r={0}", filters)));
        }

        /// <summary>
        /// Generate last page link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string LastPage(this ISearchResultsViewModel model)
        {
            var filters = HttpUtility.UrlEncode(model.SrchRequest.RawFilterStr);

            return
                string.Format("{0}?{1}&p={2}", model.PostBackUrl, 
                model.BuildQueryString(string.Format("r={0}", filters)), model.TotalPages());
        }

        /// <summary>
        /// Generate next page link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string NextPage(this ISearchResultsViewModel model)
        {
            var page = (model.SrchRequest.Page + 1 > model.TotalPages()) ? model.TotalPages() : model.SrchRequest.Page + 1;

            var filters = HttpUtility.UrlEncode(model.SrchRequest.RawFilterStr);

            return
                string.Format("{0}?{1}&p={2}", model.PostBackUrl, 
                model.BuildQueryString(string.Format("r={0}", filters)), page);
            
        }

        /// <summary>
        /// Generate previous page link
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string PrevPage(this ISearchResultsViewModel model)
        {
            var page = (model.SrchRequest.Page - 1 < 1) ? 1 : model.SrchRequest.Page - 1;

            var filters = HttpUtility.UrlEncode(model.SrchRequest.RawFilterStr);

            return
               string.Format("{0}?{1}&p={2}", model.PostBackUrl, 
               model.BuildQueryString(string.Format("r={0}", filters)), page);
           
        }

        /// <summary>
        /// Generate href to clear a filter on the search page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string BuildClearAllFiltersHref(this ISearchResultsViewModel model)
        {
            return
               string.Format("{0}?{1}", model.PostBackUrl, model.BuildQueryString("", true));
        }

        /// <summary>
        /// Generate href to clear a single filter on the search page
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fieldName"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        public static string BuildClearFilterHref(this ISearchResultsViewModel model, string fieldName, string filterValue)
        {
            var refinements = model.BuildRefinement(fieldName, filterValue);

            var queryStr = model.BuildQueryString(!string.IsNullOrEmpty(refinements) ? string.Format("r={0}", refinements) : "");

            return string.Format("{0}?{1}", model.PostBackUrl, queryStr);
        }

        /// <summary>
        /// Displays friendly version of filter values
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filterField"></param>
        /// <param name="filterValue"></param>
        /// <returns></returns>
        public static string FilterDisplayValue(this ISearchResultsViewModel model, string filterField, string filterValue)
        {
            if(filterField == "SearchPublishDate")
            {
                var dateStrs = filterValue.Split("to".ToCharArray());

                if (dateStrs.Count() == 3)
                {
                    DateTime from;
                    DateTime to;

                    DateTime.TryParse(dateStrs[0], out from);
                    DateTime.TryParse(dateStrs[2], out to);
                    return BuildRangeDateLabel(from, to);
                }
            }

            if(model.FacetTitleMap != null && model.FacetTitleMap.ContainsKey(filterValue))
            {
                return model.FacetTitleMap[filterValue];
            }

            if (model.AllTitleMap != null && model.AllTitleMap.ContainsKey(filterValue))
            {
                return model.AllTitleMap[filterValue];
            }

            // if value is a category then strip out category path
            if (filterValue.Contains("~"))
            {
                return filterValue.Split('~').Last();
            }

            return filterValue;
            

        }

        /// <summary>
        /// Determines if terms is a currently selected filter
        /// </summary>
        /// <param name="currentFilters"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        private static bool IsSelected(SearchFilters currentFilters, string term)
        {
        
            foreach(var filter in currentFilters)
            {
                if(filter.Value.Contains(term))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Converts date range to a string
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private static string DateRangeToStr(DateTime? start, DateTime? end)
        {
            var startStr = start != null ? start.Value.ToShortDateString() : "";
            var endStr = end != null ? end.Value.ToShortDateString() : "";

            return string.Format("{0}to{1}", startStr, endStr);
        }

        /// <summary>
        /// Creates a friendly label for a date range
        /// </summary>
        /// <returns></returns>
        private static string BuildRangeDateLabel(DateTime? rangeFrom, DateTime? rangeTo)
        {
            if (rangeFrom != null && rangeTo != null)
            {
                if (rangeFrom.Value == DateTime.MinValue)
                {
                    return string.Format("{0} or earlier", rangeTo.Value.Year);
                }
                else if (rangeFrom.Value.Date.Day == 1 && rangeFrom.Value.Date.Month == 1)
                {
                    return rangeFrom.Value.Year.ToString();
                }
                else if (rangeFrom.Value.Date == DateTime.Today)
                {
                    var span = rangeTo.Value - rangeFrom.Value;

                    return string.Format("Next {0} Days", span.Days);
                }
                else
                {
                    var span = rangeTo.Value - rangeFrom.Value;

                    return string.Format("Last {0} Days", span.Days);
                }

            }

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        public static string BuildLinkTarget(this UnifiedSearchHit hit)
        {
            return (hit.TypeName == null) ? "target=\"_blank\"" : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string DisplaySearchQueryText(this ISearchResultsViewModel model)
        {
            if (!string.IsNullOrEmpty(model.SrchRequest.QueryText))
            {
                return (!string.IsNullOrEmpty(model.SrchRequest.SearchWithinText)) ?
                    string.Format("\"{0}\" \"{1}\"", model.SrchRequest.QueryText, model.SrchRequest.SearchWithinText) :
                    string.Format("\"{0}\"", model.SrchRequest.QueryText);

            }

            return !string.IsNullOrEmpty(model.SrchRequest.SearchWithinText) ?
                    string.Format("\"{0}\"", model.SrchRequest.SearchWithinText) :
                    string.Empty;

        }

        /// <summary>
        /// strip html from string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveHtml(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return Regex.Replace(HttpUtility.HtmlDecode(str), @"<[^>]*>", "");
        }

        /// <summary>
        /// Get underlying object from unified search hit
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hit"></param>
        /// <returns></returns>
        public static T GetOriginalObject<T>(this UnifiedSearchHit hit)
        {
            if (hit.OriginalObjectGetter != null)
            {
                var original = hit.OriginalObjectGetter.Invoke();

                if (original is T)
                {
                    return (T)original;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Intialize search page view model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public static void IntializeSearchResults(this ISearchResultsViewModel model,
            UnifiedSearchResults results)
            
        {
            model.Results = results;
            model.TotalMatching = (results == null) ? 0 : results.TotalMatching;
            model.Facets = model.CurrentSearchPage.Facets.GetCategoryNames();
            model.End = (model.SrchRequest.Start + model.SrchRequest.PageSize) > model.TotalMatching ? model.TotalMatching : (model.SrchRequest.Start + model.SrchRequest.PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="request"></param>
        /// <param name="currentPage"></param>
        public static void IntializeViewModel(this ISearchResultsViewModel model, 
            SearchRequest request,
            ISearchPage currentPage)
        {
            model.SrchRequest = request;
            model.CurrentSearchPage = currentPage;
            model.CurrentFilters = model.SrchRequest.CurrentFilters;
            model.Start = model.SrchRequest.Start + 1;
            model.PostBackUrl = HttpContext.Current.Request.Url.AbsolutePath;
            model.DefaultSort = currentPage.DefaultSort;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public static string GetDidYouMeanUrl(this ISearchResultsViewModel model, string term)
        {
            return string.Format("{0}?id={1}", model.PostBackUrl, term); 
        }
                
    }
  }