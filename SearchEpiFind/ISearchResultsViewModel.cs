using EPiServer.Find.UnifiedSearch;
using System.Collections.Generic;

namespace SearchEpiFind.Business.Search
{
    public interface ISearchResultsViewModel
    {
        Dictionary<string, string> FacetTitleMap { get; set; }

        UnifiedSearchResults Results { get; set; }

        int Start { get; set; }

        int End { get; set; }

        SearchRequest SrchRequest { get; set; }

        SearchFilters CurrentFilters { get; set; }

        SearchFilters WildCardFilters { get; set; }

        string PostBackUrl { get; set; }

        //DidYouMeanResult DidYouMean { get; set; }

        bool AllCategoryFacets { get; set; }

        IList<string> Facets { get; set; }

        int TotalMatching { get; set; }

        bool ShowAllOptionInCategoryFacet { get; set; }

        Dictionary<string, string> AllTitleMap { get; set; }

        bool SortFacetsByTitle { get; set; }

        ISearchPage CurrentSearchPage { get; set; }

        string DefaultSort { get; set; }
    }
}