using EPiServer.Core;
using EPiServer.SpecializedProperties;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpiserverBaseSite.Business.CMS.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class AllowedMaxAttribute : ValidationAttribute
    {
        private int MaxAllowed;

        public AllowedMaxAttribute(int max)
        {
            MaxAllowed = max;
        }

        public override bool IsValid(object value)
        {
            if (value != null)
            {
                var contentArea = value as ContentArea;
                var collection = value as ICollection;
                var linkCollection = value as LinkItemCollection;

                var itemCnt = 
                    (contentArea != null) ? contentArea.Count :
                    (collection != null) ? collection.Count :
                    (linkCollection != null) ? linkCollection.Count : 0;

                if (itemCnt > MaxAllowed)
                {
                    ErrorMessage = $"This section is restricted to {MaxAllowed} content items";
                    return false;
                }
            }

            return true;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = base.IsValid(value, validationContext);

            if (result != null && !string.IsNullOrWhiteSpace(result.ErrorMessage))
            {
                result.ErrorMessage = $"{validationContext.DisplayName} {ErrorMessage}";
            }

            return result;
        }
    }
}