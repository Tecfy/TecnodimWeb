using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataEF.Validation
{
    public class RequiredExtra : ValidationAttribute, IClientValidatable
    {
        public string ErrorMessageRequired { get; set; }
        public string ErCharsNotPermitted { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var er = new System.Text.RegularExpressions.Regex(ErCharsNotPermitted, System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
            value = er.Replace(value.ToString(), "");

            return new RequiredAttribute().IsValid(value) ?
                ValidationResult.Success :
                new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

        }

        private static T GetValue<T>(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName);
            return (T)propertyInfo.GetValue(objectInstance, null);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "requiredextra",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("ercharsnotpermitted", ErCharsNotPermitted);
            yield return modelClientValidationRule;
        }
    }
}
