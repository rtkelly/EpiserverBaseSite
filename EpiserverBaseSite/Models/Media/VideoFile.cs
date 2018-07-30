using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Models.Media
{
    [ContentType(GUID = "6231dd74-73a8-421b-b563-c584f827a23c", Description = "")]
    [MediaDescriptor(ExtensionString = "flv,mp4,webm")]
    public class VideoFile : BaseMedia
    {
       
    }
}