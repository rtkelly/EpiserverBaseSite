using SearchEpiFind.Business.Search.Util;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying.Queries;
using EPiServer.Find.UnifiedSearch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace SearchEpiFind.Business.Search
{
    public static class UnifiedSearchBuilder
    {
        public static DateRange[] DefaultDateRange
        {
            get
            {
                return new List<DateRange>()
                {
                    new DateRange { From = DateTime.Now.AddDays(-30), To = DateTime.Now },
                    new DateRange { From = DateTime.Now.AddDays(-90), To = DateTime.Now },
                    new DateRange { From = DateTime.Now.AddDays(-180), To = DateTime.Now },
                    new DateRange { From = DateTime.Now, To = DateTime.Now.AddDays(30) },
                    new DateRange { From = DateTime.Now, To = DateTime.Now.AddDays(90) },
                    new DateRange { From = DateTime.Now, To = DateTime.Now.AddDays(180) },
                    new DateRange { From = new DateTime(DateTime.Now.Year, 1, 1, 00, 00, 00), To = new DateTime(DateTime.Now.Year+1, 1, 1, 0, 0, 0).AddSeconds(-1) },
                    new DateRange { From = new DateTime(DateTime.Now.Year-1, 1, 1, 00, 00, 00), To = new DateTime(DateTime.Now.Year, 1, 1, 0, 0, 0).AddSeconds(-1) },
                    new DateRange { From = new DateTime(DateTime.Now.Year-2, 1, 1, 00, 00, 00), To = new DateTime(DateTime.Now.Year-1, 1, 1, 0, 0, 0).AddSeconds(-1) },
                    new DateRange { From = DateTime.MinValue, To = new DateTime(DateTime.Now.Year-2, 1, 1, 0, 0, 0).AddSeconds(-1) },

                }.ToArray();
            }
        }


        /// <summary>
        /// build boolean query containing multiple sub queries
        /// </summary>
        /// <param name="search"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ApplyBoolQuery(this ITypeSearch<ISearchContent> search,
           SearchRequest request)
        {
            if (string.IsNullOrEmpty(request.QueryText) && string.IsNullOrEmpty(request.SearchWithinText))
                return search;

            return new Search<ISearchContent, BoolQuery>(search, context =>
            {
                var boolQuery = new BoolQuery();

                if (request.SearchWithin && !string.IsNullOrEmpty(request.SearchWithinText) && !string.IsNullOrEmpty(request.QueryText))
                {
                    boolQuery.Must.Add(new LowerCaseTermQuery("_all", request.QueryText));
                    boolQuery.Must.Add(new LowerCaseTermQuery("_all", request.SearchWithinText));
                    boolQuery.MinimumNumberShouldMatch = 2;
                }
                else if (!string.IsNullOrEmpty(request.SearchWithinText))
                {
                    boolQuery.Must.Add(new LowerCaseTermQuery("_all", request.SearchWithinText));
                    boolQuery.MinimumNumberShouldMatch = 1;
                }
                else
                {
                    boolQuery.Should.Add(new LowerCaseTermQuery("_all", request.QueryText));
                    boolQuery.MinimumNumberShouldMatch = 1;
                }
                                
                context.RequestBody.Query = boolQuery;
            });
                
                        
        }

        /// <summary>
        /// add date range filters to query
        /// </summary>
        /// <param name="search"></param>
        /// <param name="fromDateStr"></param>
        /// <param name="toDateStr"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ApplyDateFilters(this ITypeSearch<ISearchContent> search,
            string fromDateStr, string toDateStr)
        {
            if (string.IsNullOrEmpty(fromDateStr))
                return search;

            if (string.IsNullOrEmpty(toDateStr))
                return search;

            DateTime from;
            DateTime to;

            DateTime.TryParse(fromDateStr + " 00:00:00", out from);
            DateTime.TryParse(toDateStr + " 23:59:59", out to);

            if (to == DateTime.MinValue)
                return search;

            return search
                .Filter(x => x.SearchPublishDate.InRange(from, to));
        }

        /// <summary>
        /// exclude types from search results
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageTypes"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ExcludeSearchTypes(this ITypeSearch<ISearchContent> search, IEnumerable<string> pageTypes)
        {
            foreach (var pageType in pageTypes)
            {
                search = search
                    .Filter(x => !x.SearchTypeName.Match(pageType));
            }

            return search;
        }

        /// <summary>
        /// add current facet filters to search query
        /// </summary>
        /// <param name="search"></param>
        /// <param name="filters"></param>
        /// <param name="metaDataFields"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ApplyFilters(this ITypeSearch<ISearchContent> search,
        Dictionary<string, List<string>> filters, List<string> metaDataFields = null)
        {
            foreach (var filter in filters)
            {
                switch(filter.Key)
                {
                    case "SearchCategories":

                        foreach (var filterItem in filter.Value)
                        {
                            search = search
                                .Filter(x => x.SearchCategories.Match(filterItem));
                        }
                        break;

                    case "SearchSection":

                        foreach (var filterItem in filter.Value)
                        {
                            search = search
                                .Filter(x => x.SearchSection.Match(filterItem));
                        }
                        break;

                    case "SearchPublishDate":

                        if (filter.Value.Any())
                        {
                            var dateStrs = filter.Value.First().Split("to".ToCharArray());

                            DateTime from;
                            DateTime to;

                            DateTime.TryParse(dateStrs[0] + " 00:00:00", out from);
                            DateTime.TryParse(dateStrs[2] + " 23:59:59", out to);

                            if (to == DateTime.MinValue)
                                return search;
                                                        

                            search = search
                                .Filter(x => x.SearchPublishDate.InRange(from, to));
                        }
                        break;

                    default:

                        //"SearchMetaData.bt:publication.StringValue":
                        if(filter.Key.Contains("SearchMetaData"))
                        {
                            var split = filter.Key.Split('.');

                            if(split.Count() == 3)
                            {
                                var metaDataField = split[1];

                                foreach (var value in filter.Value)
                                {
                                    search = search
                                        .Filter(x => x.SearchMetaData[metaDataField].Match(value));
                                }
                            }
                        }
                        else if(filter.Key.StartsWith("filteron_"))
                        {
                            foreach(var value in filter.Value)
                            {
                                search = search
                                    .ProcessMetadataFilters(value, metaDataFields);
                            }
                        }

                        break;
                }
                
            }

            return search;

        }


        /// <summary>
        /// add always apply filter to search query
        /// </summary>
        /// <param name="search"></param>
        /// <param name="categoryPaths"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> AlwaysApplyFilters(this ITypeSearch<ISearchContent> search,
            IEnumerable<string> categoryPaths)
        {
             
            foreach(var categoryPath in categoryPaths)
            { 
                search = search
                    .Filter(x => x.SearchCategories.Match(categoryPath));
            }

            return search;

        }


        /// <summary>
        /// add pagination to search query
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="search"></param>
        /// <param name="start"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static ITypeSearch<TSource> ApplyPageination<TSource>(this ITypeSearch<TSource> search,
           int start, int pageSize)
        {
            return search
               .Skip(start)
               .Take(pageSize);
        }

        /// <summary>
        /// add sort to search query
        /// </summary>
        /// <param name="search"></param>
        /// <param name="sortBy"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ApplySort(this ITypeSearch<ISearchContent> search,
           string sortBy)
        {
            switch (sortBy.ToLower())
            {
                case "title":
                    return search
                        .OrderBy(x => x.SearchTitle);

                case "date":
                case "newest":
                    return search
                        .OrderByDescending(x => x.SearchPublishDate, SortMissing.Last);

                case "oldest":
                    return search
                        .OrderBy(x => x.SearchPublishDate, SortMissing.Last);

                case "pricelow":
                    return search
                        .OrderBy(x => x.SearchMetaData["listprice"].DecimalValue, SortMissing.Last);

                case "pricehigh":
                    return search
                        .OrderByDescending(x => x.SearchMetaData["listprice"].DecimalValue, SortMissing.Last);

                default:
                    return search;

            }
        }

        /// <summary>
        /// get search results with highlighting
        /// </summary>
        /// <param name="search"></param>
        /// <param name="EncodeHtml"></param>
        /// <returns></returns>
        public static UnifiedSearchResults ApplyGetResults(this ITypeSearch<ISearchContent> search, bool EncodeHtml=true)
        {
            return search.GetResult(new HitSpecification
            {
                HighlightExcerpt = true,
                PreTagForAllHighlights = "<b>",
                PostTagForAllHighlights = "</b>",
                EncodeExcerpt = EncodeHtml,
                EncodeTitle = EncodeHtml,
            });

            
        }
        
        /// <summary>
        /// create current filters from query string
        /// </summary>
        /// <param name="currentFilters"></param>
        /// <returns></returns>
        public static SearchFilters CurrentFiltersFromString(string currentFilters)
        {
            var c = new SearchFilters();

            if (string.IsNullOrEmpty(currentFilters))
                return c;

            var fields = currentFilters.Split(';');

            foreach(var field in fields)
            {
                var fieldSplit = field.Replace("]", "").Split('[');

                if (fieldSplit.Count() == 2)
                {
                    c.Add(fieldSplit[0], HttpUtility.UrlDecode(fieldSplit[1])
                        ?.Split(',')
                        .ToList());
                }

            }


            return c;
        }
                

        /// <summary>
        /// filters search query on metadata
        /// </summary>
        /// <param name="search"></param>
        /// <param name="queryText"></param>
        /// <param name="metadataFields"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ProcessMetadataFilters(this ITypeSearch<ISearchContent> search,
            string queryText, IEnumerable<string> metadataFields)
        {
            return new Search<ISearchContent, BoolQuery>(search, context =>
            {
                var boolQuery = new BoolQuery()
                {
                    MinimumNumberShouldMatch = 1
                };

                if (context.RequestBody.Query != null)
                {
                    boolQuery.Must.Add(context.RequestBody.Query);
                }
                               
                foreach (var metadata in metadataFields)
                {
                    var metadataQuery = new WildcardQuery(metadata, queryText.ToLowerInvariant());
                    boolQuery.Should.Add(metadataQuery);
                }

                context.RequestBody.Query = boolQuery;
            });
            
        }

        /// <summary>
        /// perform a wild card search query
        /// </summary>
        /// <param name="search"></param>
        /// <param name="query"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public static ITypeSearch<ISearchContent> ApplyWildCardQuery(this ITypeSearch<ISearchContent> search, string query, Expression<Func<ISearchContent, string>> field)
        {
            var fieldName = search.Client.Conventions
                .FieldNameConvention
                .GetFieldNameForAnalyzed(field);

            var wildcardQuery = new WildcardQuery(fieldName, query.ToLowerInvariant());
                        
            return new Search<ISearchContent, BoolQuery>(search, context =>
            {
                if (context.RequestBody.Query != null)
                {
                    var boolQuery = new BoolQuery();
                    boolQuery.Must.Add(context.RequestBody.Query);
                    boolQuery.Should.Add(wildcardQuery);
                    boolQuery.MinimumNumberShouldMatch = 1;
                    context.RequestBody.Query = boolQuery;
                }
                else
                {
                    context.RequestBody.Query = wildcardQuery;
                }
            });
        }

        /// <summary>
        /// get wild card filters from search page
        /// </summary>
        /// <param name="filterCategories"></param>
        /// <returns></returns>
        public static SearchFilters BuildWildCardFilters(this CategoryList filterCategories)
        {
            var filters = new SearchFilters();

            foreach (var category in filterCategories.GetCategoryListData())
            {
                var values = new List<string>();

                foreach (var value in category.Categories)
                {
                    values.Add(value.Name + "~" + value.Description);
                }

                filters.Add(category.Name, values);
            }

            return filters;
        }
               

    }
}