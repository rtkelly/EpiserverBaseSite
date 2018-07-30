using System.Collections.Generic;

namespace SearchEpiFind.Business.Search
{
    public class Pagination
    {
        public int Page { get; set; }
        public string Link { get; set; }
        public bool Selected { get; set; }


        public static List<int> GetPagination(int totalPages, int currentPage, int groupSize = 10)
        {
            var list = new List<int>();
            
            if (totalPages > groupSize + 1)
            {
                int groupHalf = groupSize / 2;

                int pageStart = (currentPage - groupHalf);
                int pageEnd = (currentPage + groupHalf);

                if (pageStart < 1)
                {
                    pageEnd = pageEnd - pageStart;
                    pageStart = 1;
                }

                if (pageEnd >= totalPages)
                {
                    pageStart = pageStart - (pageEnd - totalPages);
                    pageEnd = totalPages;
                }

                if (pageEnd > totalPages)
                    pageEnd = totalPages;

                for (int page = pageStart; page <= pageEnd; page++)
                {
                    list.Add(page);
                }
            }
            else if (totalPages > 1)
            {
                for (int page = 1; page <= totalPages; page++)
                {
                    list.Add(page);
                }
            }


            return list;
        }
    }


}