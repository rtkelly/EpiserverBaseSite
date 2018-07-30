using EPiServer.DataAnnotations;
using EPiServer.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.CMS
{
   

        [GroupDefinitions]
        public static class CmsTabNames

        {
            [Display(Order = 10)]
            [RequiredAccess(AccessLevel.Edit)]
            public const string Header = "Header";

            [Display(Order = 20)]
            [RequiredAccess(AccessLevel.Edit)]
            public const string Footer = "Footer";

            [Display(Order = 30)]
            [RequiredAccess(AccessLevel.Edit)]
            public const string MetaData = "Metadata";

            [Display(Order = 40)]
            [RequiredAccess(AccessLevel.Edit)]
            public const string Scripts = "Scripts";



        }
    
}