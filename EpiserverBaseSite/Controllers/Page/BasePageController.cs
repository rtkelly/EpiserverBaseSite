using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Controllers
{
    public class BasePageController<T> : PageController<T> where T : PageData
    {
        protected readonly Injected<SiteServices> Site;
       
      
    }
}