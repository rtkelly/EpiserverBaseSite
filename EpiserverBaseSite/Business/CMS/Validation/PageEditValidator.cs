using EPiServer.Core;
using EPiServer.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.CMS.Validation
{
    public class PageEditValidator : IValidate<PageData>
    {
        public IEnumerable<ValidationError> Validate(PageData instance)
        {
            var errors = new List<ValidationError>();

            

            return errors;
        }
    }
}