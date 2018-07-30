using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EpiserverBaseSite.Models.Blocks;

namespace EpiserverBaseSite.Controllers.Block
{
    public class CalloutBlockController: BlockControllerBase<CalloutBlock>
    {
        private static readonly Injected<IDisplayChannelService> _displayChannelService;


        public override ActionResult Index(CalloutBlock currentBlock)
        {
            //var logo = Site.Service.Settings.Logo;
            
            var isMobile = _displayChannelService.Service
                .GetActiveChannels(new HttpContextWrapper(System.Web.HttpContext.Current))
                .Any(c => String.Equals(c.ChannelName, RenderingTags.Mobile, StringComparison.OrdinalIgnoreCase));

            return PartialView(isMobile ? "CalloutBlockMobile" : "CalloutBlock", currentBlock);
        }
    }
}
