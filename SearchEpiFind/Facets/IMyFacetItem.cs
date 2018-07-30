namespace SearchEpiFind.Business.Search
{
    public interface IMyFacetItem
    {
        string Title { get; set; }

        string FieldName { get; set; }

        string Term { get; set; }

        int Count { get; set; }

        bool Selected { get; set; }

        string Filter { get; set; }

        int Sort { get; set; }
    }
}