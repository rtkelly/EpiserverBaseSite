using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EpiserverBaseSite.Business.CMS;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Models.Abstract
{
    public abstract class BaseMedia : MediaData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 100)]
        [CultureSpecific]
        public virtual string DisplayTitle { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 200)]
        [CultureSpecific]
        public virtual String Summary { get; set; }
    }
}