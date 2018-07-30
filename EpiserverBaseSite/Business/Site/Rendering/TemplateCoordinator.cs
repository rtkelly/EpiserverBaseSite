using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Controllers.Page;
using EpiserverBaseSite.Models.Abstract;
using EpiserverBaseSite.Models.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Rendering
{
    [ServiceConfiguration(typeof(IViewTemplateModelRegistrator))]
    public class TemplateCoordinator : IViewTemplateModelRegistrator
    {
        public const string BlockFolder = "~/Views/Shared/Blocks/";
        public const string PagePartialsFolder = "~/Views/Shared/PagePartials/";

        public static void OnTemplateResolved(object sender, TemplateResolverEventArgs args)
        {
            //Disable DefaultPageController for page types that shouldn't have any renderer as pages
            if (args.ItemToRender is IContainerPage && args.SelectedTemplate != null 
                && args.SelectedTemplate.TemplateType == typeof(DefaultPageController))
            {
                args.SelectedTemplate = null;
            }
        }


        public void Register(TemplateModelCollection viewTemplateModelRegistrator)
        {
         
            /*
            viewTemplateModelRegistrator.Add(typeof(CalloutBlock), new TemplateModel
            {
                AvailableWithoutTag = true,
                Path = BlockPath("CalloutBlock.cshtml")
            });
            */
            
        }

        private static string BlockPath(string fileName)
        {
            return string.Format("{0}{1}", BlockFolder, fileName);
        }

        private static string PagePartialPath(string fileName)
        {
            return string.Format("{0}{1}", PagePartialsFolder, fileName);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public static void MoibleTemplateResolver<T>(object sender, TemplateResolverEventArgs eventArgs) where T : PageData
        {
            if (eventArgs.ItemToRender != null && eventArgs.ItemToRender is T &&
                HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                var mobileRender = eventArgs.SupportedTemplates
                    .SingleOrDefault(r => r.Name.Contains("Mobile") &&
                    r.TemplateTypeCategory == eventArgs.SelectedTemplate.TemplateTypeCategory);

                if (mobileRender != null)
                {
                    eventArgs.SelectedTemplate = mobileRender;
                }
            }
        }
    }
}