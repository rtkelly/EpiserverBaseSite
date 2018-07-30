using EPiServer;
using EPiServer.Core;
using EPiServer.SpecializedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Models.Abstract
{
    public interface ISiteSettings
    {
        string Logo { get; set; }

        Url SearchPage { get; set; }
        
        LinkItemCollection UtilityMenu { get; set; }
                        
        ContentArea MainMenu { get; set; }

        LinkItemCollection SocialMenu { get; set; }

        ContentArea HeaderScripts { get; set; }

        ContentArea BodyScripts { get; set; }

        ContentArea FooterScripts { get; set; }
    }
}