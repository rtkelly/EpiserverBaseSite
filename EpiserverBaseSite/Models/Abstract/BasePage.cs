using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EpiserverBaseSite.Business.CMS;
using EpiserverBaseSite.Business.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace EpiserverBaseSite.Models.Abstract
{
    public abstract class BasePage : PageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 100)]
        [CultureSpecific]
        public virtual string DisplayTitle { get; set; }

        [Display(GroupName = CmsTabNames.MetaData, Order = 100)]
        [CultureSpecific]
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                // Use explicitly set meta title, otherwise fall back to page name
                return !string.IsNullOrWhiteSpace(metaTitle)
                        ? metaTitle
                        : this.GetTitle();
            }
            set { this.SetPropertyValue(p => p.MetaTitle, value); }
        }

    [Display(GroupName = CmsTabNames.MetaData, Order = 200)]
        [CultureSpecific]
        [BackingType(typeof(PropertyStringList))]
        public virtual string[] MetaKeywords { get; set; }

        [Display(GroupName = CmsTabNames.MetaData, Order = 300)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string MetaDescription { get; set; }

        [Display(GroupName = CmsTabNames.MetaData, Order = 400)]
        [CultureSpecific]
        public virtual bool DisableIndexing { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 100)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference PageImage { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 200)]
        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        public virtual string Summary { get; set; }
        
        [Display(GroupName = SystemTabNames.Settings, Order = 300)]
        [CultureSpecific]
        public virtual bool HideSiteHeader { get; set; }

        [Display(GroupName = SystemTabNames.Settings, Order = 400)]
        [CultureSpecific]
        public virtual bool HideSiteFooter { get; set; }

        public string ContentAreaCssClass
        {
            get { return "teaserblock"; } 
        }

    }
}