using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Business.Extensions;
using EpiserverBaseSite.Models.Abstract;
using EpiserverBaseSite.Models.ViewModels;

namespace EpiserverBaseSite.Controllers.Page
{
    [TemplateDescriptor(Inherited = true)]
    public class PagePartialController : PartialContentController<BasePage>
    {
        public override ActionResult Index(BasePage currentPage)
        {

            return PartialView("PageTeaser", currentPage.GetTeaser());
        }
    }

    [TemplateDescriptor(Inherited = true, Tags = new string[] { "HomeFeatured" })]
    public class HomePagePartialController : PartialContentController<BasePage>
    {
        public override ActionResult Index(BasePage currentPage)
        {

            return PartialView("HomePageTeaser", currentPage.GetTeaser());
        }
    }
}