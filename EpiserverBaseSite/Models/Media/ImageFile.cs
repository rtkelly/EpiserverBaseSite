using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Models.Media
{
    [ContentType(GUID = "c5068d36-34e4-4a4d-8434-d353edb371ce", Description = "")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : BaseMedia
    {
        public virtual string Copyright { get; set; }
    }
}