using EPiServer.Find.Api.Querying.Queries;

namespace SearchEpiFind.Business.Search
{
    public class LowerCaseTermQuery : TermQuery
    {

        public LowerCaseTermQuery(string field, string value) : base(field, value.ToLowerInvariant())
        {
        
        }
    }
}