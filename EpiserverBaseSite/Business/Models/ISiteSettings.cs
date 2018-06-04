using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Models
{
    public interface ISiteSettings
    {
        string Logo { get; set; }

        ContentArea HeaderScripts { get; set; }

        ContentArea BodyScripts { get; set; }

        ContentArea FooterScripts { get; set; }
    }
}