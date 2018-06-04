using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiserverBaseSite.Business.Infrastructure
{
    public class SiteViewEngine : RazorViewEngine
    {
        private const string PagesFolder = "~/Views/Pages/";
        private const string BlocksFolder = "~/Views/Blocks/";

        private static readonly string[] AdditionalPartialViewFormats = {
            BlocksFolder + "{0}.cshtml",
        };

        private static readonly string[] AdditionalPageViewFormats =
        {
            PagesFolder + "{1}/{0}.cshtml",
            PagesFolder + "{1}.cshtml",
            PagesFolder + "{0}.cshtml"
        };

        public SiteViewEngine()
        {
            PartialViewLocationFormats = PartialViewLocationFormats.Union(AdditionalPartialViewFormats).ToArray();
            ViewLocationFormats = ViewLocationFormats.Union(AdditionalPageViewFormats).ToArray();
        }
    }
}