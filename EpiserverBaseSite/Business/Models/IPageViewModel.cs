using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Models
{
    public interface IPageViewModel<out T> where T : PageData
    {
        T CurrentPage { get; }

        ISiteSettings SiteSettings { get; set; }

    }
}