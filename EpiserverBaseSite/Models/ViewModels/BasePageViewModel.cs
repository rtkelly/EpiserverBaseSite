using EPiServer.Core;
using EpiserverBaseSite.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Models.ViewModels
{
    public class BasePageViewModel<T> : IPageViewModel<T> where T : PageData
    {
        public T CurrentPage { get; set; }

        public ISiteSettings SiteSettings { get; set; }

    }
}