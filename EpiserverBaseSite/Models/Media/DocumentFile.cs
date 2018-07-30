using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;
using EpiserverBaseSite.Models.Abstract;

namespace EpiserverBaseSite.Models.Media
{
    [ContentType(GUID = "18f7dfb1-da2a-4e13-bbec-5bc8ca8b1849")]
    [MediaDescriptor(ExtensionString = "pdf,doc,docx")]
    public class DocumentFile : BaseMedia
    {

    }
}