using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Business.Models;
using EpiserverBaseSite.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Controllers.Block
{
    public class BaseBlockController<T> : BlockController<T> where T : BlockData
    {
        protected readonly Injected<SiteServices> Site;
    }
}