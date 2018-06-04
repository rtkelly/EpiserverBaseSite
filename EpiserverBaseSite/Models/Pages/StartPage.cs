using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EpiserverBaseSite.Business.Models;
using EpiserverBaseSite.Business.Models.Blocks;

namespace EpiserverBaseSite.Models.Pages
{
    [ContentType(DisplayName = "Start Page", GUID = "76367dd9-5836-47c4-80f8-d71eca69018e", Description = "")]
    public class StartPage : BasePage, ISiteSettings
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Logo { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Body { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual ContentArea Content { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [AllowedTypes(new[] { typeof(ScriptBlock) })]
        public virtual ContentArea HeaderScripts { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [AllowedTypes(new[] { typeof(ScriptBlock) })]
        public virtual ContentArea BodyScripts { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [AllowedTypes(new[] { typeof(ScriptBlock) })]
        public virtual ContentArea FooterScripts { get; set; }
    }
}