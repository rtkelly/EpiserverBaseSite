using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Models.Pages
{
    [ContentType(GUID = "3f934157-41b3-4259-8045-a4a57aac720c", Description = "")]
    public class StandardPage : BasePage
    {
        
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 1)]
        public virtual XhtmlString MainBody { get; set; }
         
    }
}