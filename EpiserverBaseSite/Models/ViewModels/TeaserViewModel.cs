using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Models.ViewModels
{
    public class TeaserViewModel
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public LinkViewModel Url { get; set; }

        public ImageViewModel Image { get; set; }
    }
}