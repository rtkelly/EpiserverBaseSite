using System.Collections.Generic;

namespace SearchEpiFind.Business.Search
{
    public interface IMyFacet
    {
        string Name { get; set; }

        IEnumerable<IMyFacetItem> Terms { get; set; }

    }
}