using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;

namespace EpiserverBaseSite.Business.Models.Blocks
{
    [ContentType(DisplayName = "Script Block", GUID = "9ef567b8-234a-4f3e-8e28-bcd385c2d95a", Description = "")]
    public class ScriptBlock : BlockData
    {

        [CultureSpecific]
        [UIHint(UIHint.Textarea)]
        [Display(Order = 10)]
        public virtual string Script { get; set; }
         
    }
}