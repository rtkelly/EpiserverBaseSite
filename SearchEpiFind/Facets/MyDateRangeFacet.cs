using EPiServer.Find.Api.Facets;
using System.Collections.Generic;

namespace SearchEpiFind.Business.Search
{
    public class MyDateRangeFacet : Facet, IMyFacet
    {
        public IEnumerable<IMyFacetItem> Terms { get; set; }
    }
}