using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EpiserverBaseSite.Models.Blocks
{
    [ContentType(GUID = "581d632e-c22f-4767-84e5-dc201b70236b")]
    public class HeroBlock : BlockData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual XhtmlString Body { get; set; }
    }
}