using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataEF.Validation
{
    public class RequiredIfTrueAttribute : ValidationAttribute, IClientValidatable
    {
        public string BooleanPropertyName { get; set; }

        public bool number { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                return ValidationResult.Success;
            }

            if (GetValue<bool>(validationContext.ObjectInstance, BooleanPropertyName))
            {
                if (number)
                {
                    double test = 0;

                    if (double.TryParse(value.ToString(), out test))
                    {
                        if (test > 0)
                        {
                            return ValidationResult.Success;
                        }
                    }

                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                return new RequiredAttribute().IsValid(value) ?
                    ValidationResult.Success :
                    new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
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
                ValidationType = "requirediftrue",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("boolprop", BooleanPropertyName);
            modelClientValidationRule.ValidationParameters.Add("number", number);
            yield return modelClientValidationRule;
        }
    }
}