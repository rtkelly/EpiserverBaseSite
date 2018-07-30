using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Models.Pages
{
    [ContentType(GUID = "47fc92f1-5dd9-4e39-b419-85155cca10ef")]
    public class ContainerPage : PageData, IContainerPage
    {
        
    }
}