using EPiServer.Find.Api.Facets;
using System.Collections.Generic;

namespace SearchEpiFind.Business.Search
{
    public class MyTermsFacet : Facet, IMyFacet
    {        
        public IEnumerable<IMyFacetItem> Terms { get; set; }
    }
}