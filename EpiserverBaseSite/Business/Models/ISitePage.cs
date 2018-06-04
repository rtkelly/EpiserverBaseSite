using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.Models
{
    public interface ISitePage
    {
        string PageTitle { get; set; }

        string PageSummary { get; set; }

        DateTime PublicationDate { get; set; }
    }
}