using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EpiserverBaseSite.Models.Blocks
{
    [ContentType(DisplayName = "Callout", GUID = "fe080ddb-4fd6-4a45-93f0-b8596860a4d7", Description = "")]
    public class CalloutBlock : BlockData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Body { get; set; }
    }
}