using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Business.Site;
using EpiserverBaseSite.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Controllers
{
    public class PageControllerBase<T> : PageController<T> where T : PageData
    {
        protected readonly Injected<SiteServices> Site;


        protected ISiteSettings SiteSettings { get { return Site.Service.Settings; } }

    }
}