using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace EpiserverBaseSite.Business.Models.Menu
{
    [ContentType(DisplayName = "MegaMenuBlock", GUID = "535c7577-dc0b-41c1-a8ce-e01d0c405ed7", Description = "")]
    public class MegaMenuBlock : BlockData
    {
        /*
                [CultureSpecific]
                [Display(
                    Name = "Name",
                    Description = "Name field's description",
                    GroupName = SystemTabNames.Content,
                    Order = 1)]
                public virtual string Name { get; set; }
         */
    }
}