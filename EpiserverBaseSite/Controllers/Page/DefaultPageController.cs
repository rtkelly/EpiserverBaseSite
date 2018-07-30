using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Models.Abstract;
using EpiserverBaseSite.Models.ViewModels;

namespace EpiserverBaseSite.Controllers.Page
{
    [TemplateDescriptor(Inherited = true)]
    public class DefaultPageController : PageControllerBase<BasePage>
    {
        
        public ActionResult Index(BasePage currentPage)
        {
            var model = CreateModel(currentPage);
            return View(string.Format("~/Views/{0}/Index.cshtml", currentPage.GetOriginalType().Name), model);
                        
        }
      

        private PageViewModel<BasePage> CreateModel(BasePage page)
        {
            var type = typeof(PageViewModel<>).MakeGenericType(page.GetOriginalType());
            return Activator.CreateInstance(type, page, SiteSettings) as PageViewModel<BasePage>;
        }
    }
}