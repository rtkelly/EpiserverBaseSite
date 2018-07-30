using EPiServer.Core;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Models.ViewModels
{
    public class PageViewModel<T> where T : PageData
    {
        public T CurrentPage { get; set; }

        public ISiteSettings SiteSettings { get; set; }

        public PageViewModel(T currentPage, ISiteSettings settings)
        {
            CurrentPage = currentPage;
            SiteSettings = settings;
        }

    }
}