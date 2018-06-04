using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web;
using EPiServer.Web.Mvc;
using EpiserverBaseSite.Models.Blocks;

namespace EpiserverBaseSite.Controllers.Block
{
    public class CalloutBlockController: BaseBlockController<CalloutBlock>
    {
        public override ActionResult Index(CalloutBlock currentBlock)
        {
            var logo = Site.Service.Settings.Logo;

            return PartialView("Callout", currentBlock);
        }
    }
}
