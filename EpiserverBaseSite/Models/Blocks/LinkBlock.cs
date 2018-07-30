using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Business.Models.Blocks
{
    [ContentType(DisplayName = "Link Block", GUID = "1fb8d23b-887b-45e0-84de-62f419eb71d6", Description = "")]
    public class LinkBlock : BaseBlock
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