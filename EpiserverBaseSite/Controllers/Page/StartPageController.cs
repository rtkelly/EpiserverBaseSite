using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Models.Pages;
using EpiserverBaseSite.Models.ViewModels;

namespace EpiserverBaseSite.Controllers.Page
{
    public class StartPageController : PageControllerBase<StartPage>
    {
        public ActionResult Index(StartPage currentPage)
        {
            var viewModel = new StartPageViewModel(currentPage, Site.Service.Settings);
            
            

            return View(viewModel);
        }
    }
}