using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EpiserverBaseSite.Business.Rendering;
using EpiserverBaseSite.Business.Site.Channels;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DisplayModesInitialization : IInitializableModule
    {
    
        public void Initialize(InitializationEngine context)
        {
           
        }
               

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.TemplateResolver().TemplateResolved
               -= new EventHandler<TemplateResolverEventArgs>(
                   TemplateCoordinator.MoibleTemplateResolver<BasePage>);
        }

       
    }
}