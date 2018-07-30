using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchEpiFind.Business.Search
{
    public interface ISearchPage
    {
        //CategoryList AlwaysApplyFilter { get; set; }

        CategoryList Facets { get; set; }

        string DefaultSort { get; set; }

        // Facets { get; set; }

        //XhtmlString NoResults { get; set; }

        //bool EnableRelatedQueries { get; set; }

    }
}