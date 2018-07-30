using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EpiserverBaseSite.Business.Rendering
{
    public class SiteViewEngine : RazorViewEngine
    {
        private const string PagesFolder = "~/Views/Pages/";
        private const string BlocksFolder = "~/Views/Shared/Blocks/";
        private const string PartialViewsFolder = "~/Views/Shared/PartialViews/";


        private static readonly string[] AdditionalPartialViewFormats = {
            BlocksFolder + "{0}.cshtml",
            PartialViewsFolder + "{0}.cshtml",

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