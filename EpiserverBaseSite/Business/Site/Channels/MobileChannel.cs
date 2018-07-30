using EPiServer.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace EpiserverBaseSite.Business.Site.Channels
{

    public class MobileChannel : DisplayChannel
    {

        public const string Name = "mobile";

        public override string ChannelName
        {
            get
            {
                return Name;
            }
        }

        public override string ResolutionId
        {
            get
            {
                return typeof(IphoneVerticalResolution).FullName;
            }
        }

        public override bool IsActive(HttpContextBase context)
        {
            return context.GetOverriddenBrowser().IsMobileDevice;
        }

        
    }
}