using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EpiserverBaseSite.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EpiserverBaseSite.Business.Extensions;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Business.Site
{
    [ServiceConfiguration(typeof(SiteServices), Lifecycle = ServiceInstanceScope.HttpContext)]
    public class SiteServices  
    {
        private ISiteSettings _siteSettings;

        public ISiteSettings Settings
        {
            get
            {
                if (_siteSettings == null)
                    _siteSettings = LoadSiteSettings();

                return _siteSettings;
            }
        }

        private ISiteSettings LoadSiteSettings()
        {
            var settingsPage = ContentReference.StartPage.TryGet<PageData>();
            
            return settingsPage as ISiteSettings;
        }
    }
}