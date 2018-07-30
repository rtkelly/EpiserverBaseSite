using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EpiserverBaseSite.Business.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiserverBaseSite.Business.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class CustomizedRenderingInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            ViewEngines.Engines.Insert(0, new SiteViewEngine());

            //context.Locate.TemplateResolver()
            //    .TemplateResolved += TemplateCoordinator.OnTemplateResolved;
        }

        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}