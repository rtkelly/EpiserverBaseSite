using EPiServer.Find.Api.Facets;

namespace SearchEpiFind.Business.Search
{
    public class MyDateRangeResult : DateRangeResult, IMyFacetItem
    {
        public string Title { get; set; }

        public string FieldName { get; set; }

        public string Term { get; set; }

        //public int Count { get; set; }

        public bool Selected { get; set; }

        public string Filter { get; set; }

        public int Sort { get; set; }

    }
}