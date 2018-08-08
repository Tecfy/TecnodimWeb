using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace DataEF.Validation
{
    public class RequiredIfValueAttribute : ValidationAttribute, IClientValidatable
    {
        public string ValuePropertyName { get; set; }

        public string values { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                return ValidationResult.Success;
            }

            var value2 = GetValue(validationContext.ObjectInstance, ValuePropertyName);
            var temp = values.ToString().Split('|');
            if (temp.Contains(value2))
            {
                return new RequiredAttribute().IsValid(value) ?
                    ValidationResult.Success :
                    new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }

        private static string GetValue(object objectInstance, string propertyName)
        {
            if (objectInstance == null) throw new ArgumentNullException("objectInstance");
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");

            var propertyInfo = objectInstance.GetType().GetProperty(propertyName.Split('_').Last());


            return propertyInfo.GetValue(objectInstance, null).ToString();

        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var modelClientValidationRule = new ModelClientValidationRule
            {
                ValidationType = "requiredifvalue",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("valueprop", ValuePropertyName);
            modelClientValidationRule.ValidationParameters.Add("values", values);
            yield return modelClientValidationRule;
        }
    }
}