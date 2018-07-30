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

namespace EpiserverBaseSite.Business.Site.Initialization
{
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DisplayModesInitialization : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            // register mobile display mode
            context.Locate
               .DisplayChannelService()
               .RegisterDisplayMode(new DefaultDisplayMode("mobile")
               {
                   ContextCondition = (r) => r.Request.Browser.IsMobileDevice
               });

            /*
              var mobileChannelDisplayMode = new DefaultDisplayMode("mobile")
            {
                ContextCondition = IsMobileDisplayModeActive
            };
            DisplayModeProvider.Instance.Modes.Insert(0, mobileChannelDisplayMode);
             */

            // programmatically resolve moible template 
            context.Locate.TemplateResolver().TemplateResolved
                += new EventHandler<TemplateResolverEventArgs>(
                    TemplateCoordinator.MoibleTemplateResolver<BasePage>);


            // Register Display Options for content areas
            if (context.HostType == HostType.WebApplication)
            {
                var options = ServiceLocator.Current.GetInstance<DisplayOptions>();
                options
                    .Add("full", "/displayoptions/full", Global.ContentAreaTags.FullWidth, "", "epi-icon__layout--full")
                    .Add("wide", "/displayoptions/wide", Global.ContentAreaTags.TwoThirdsWidth, "", "epi-icon__layout--two-thirds")
                    .Add("narrow", "/displayoptions/narrow", Global.ContentAreaTags.OneThirdWidth, "", "epi-icon__layout--one-third");

                AreaRegistration.RegisterAllAreas();

            }

           
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.Locate.TemplateResolver().TemplateResolved
               -= new EventHandler<TemplateResolverEventArgs>(
                   TemplateCoordinator.MoibleTemplateResolver<BasePage>);

        }

        private static bool IsMobileDisplayModeActive(HttpContextBase httpContext)
        {
            if (httpContext.GetOverriddenBrowser().IsMobileDevice)
            {
                return true;
            }
            var displayChannelService = ServiceLocator.Current.GetInstance<IDisplayChannelService>();
            return displayChannelService.GetActiveChannels(httpContext).Any(x => x.ChannelName == MobileChannel.Name);
        }

        

       
    }
}