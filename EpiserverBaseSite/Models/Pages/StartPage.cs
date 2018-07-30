using System;
using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EpiserverBaseSite.Business.CMS;
using EpiserverBaseSite.Business.Models;
using EpiserverBaseSite.Business.Models.Blocks;
using EpiserverBaseSite.Models.Abstract;
using EpiserverBaseSite.Models.Blocks;

namespace EpiserverBaseSite.Models.Pages
{
    [ContentType(GUID = "76367dd9-5836-47c4-80f8-d71eca69018e")]
    public class StartPage : BasePage, ISiteSettings
    {
        [Display(GroupName = CmsTabNames.Header, Order = 10)]
        public virtual string Logo { get; set; }

        [Display(GroupName = CmsTabNames.Header, Order = 20)]
        public virtual LinkItemCollection UtilityMenu { get; set; }

        [Display(GroupName = CmsTabNames.Header, Order = 30)]
        public virtual ContentArea MainMenu { get; set; }

        [Display(GroupName = CmsTabNames.Header, Order = 40)]
        public virtual Url SearchPage { get; set; }

        [Display(GroupName = CmsTabNames.Header, Order = 50)]
        public virtual LinkItemCollection SocialMenu { get; set; }

        [AllowedTypes(new[] { typeof(HeroBlock) })]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual ContentArea Hero { get; set; }

        [AllowedTypes(new[] { typeof(BasePage) })]
        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual ContentArea FeaturedPages { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual XhtmlString Body { get; set; }
        
        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual ContentArea MainContent { get; set; }

        [Display(GroupName = CmsTabNames.Scripts, Order = 10)]
        [AllowedTypes(new[] { typeof(ScriptBlock) })]
        public virtual ContentArea HeaderScripts { get; set; }

        [Display(GroupName = CmsTabNames.Scripts, Order = 20)]
        [AllowedTypes(new[] { typeof(ScriptBlock) })]
        public virtual ContentArea BodyScripts { get; set; }

        [Display(GroupName = CmsTabNames.Scripts, Order = 30)]
        [AllowedTypes(new[] { typeof(ScriptBlock) })]
        public virtual ContentArea FooterScripts { get; set; }

        
        

        
    }
}