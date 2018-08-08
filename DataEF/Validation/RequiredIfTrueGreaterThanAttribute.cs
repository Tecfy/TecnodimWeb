using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DataEF.Validation
{
    public class RequiredIfTrueGreaterThanAttribute : ValidationAttribute, IClientValidatable
    {
        public string BooleanPropertyName { get; set; }

        public string GreaterThanPropertyName { get; set; }

        public Type TypeName { get; set; }

        public RequiredIfTrueGreaterThanAttribute()
        {
            TypeName = typeof(double);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (validationContext == null)
            {
                return ValidationResult.Success;
            }

            if (GetValue<bool>(validationContext.ObjectInstance, BooleanPropertyName))
            {
                var Valid = new RequiredIfTrueAttribute()
                {
                    BooleanPropertyName = BooleanPropertyName,
                    number = true
                }.IsValid(value);


                if (!Valid)
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }

                

                if (TypeName.Equals(typeof(double)))
                {


                    var value2 = GetValue<decimal>(validationContext.ObjectInstance, GreaterThanPropertyName);
                    double test2 = 0;
                    double test1 = 0;

                    if (double.TryParse(value2.ToString(), out test2) && double.TryParse(value.ToString(), out test1))
                    {
                        if (test1 <= test2)
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
                    }
                    else
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }

                else if (TypeName.Equals(typeof(DateTime)))
                {

                    var value2 = GetValue<DateTime>(validationContext.ObjectInstance, GreaterThanPropertyName);
                    DateTime test2 = DateTime.MinValue;
                    DateTime test1 = DateTime.MinValue;

                    if (DateTime.TryParse(value2.ToString(), out test2) && DateTime.TryParse((String.IsNullOrEmpty(value.ToString()) ? DateTime.MaxValue.ToString() : value.ToString()), out test1))
                    {
                        if (test1 < test2)
                        {
                            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                        }
                    }
                    else
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }


                }


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
                ValidationType = "requirediftruegreaterthan",
                ErrorMessage = FormatErrorMessage(metadata.DisplayName)
            };
            modelClientValidationRule.ValidationParameters.Add("boolprop", BooleanPropertyName);
            modelClientValidationRule.ValidationParameters.Add("greaterthan", GreaterThanPropertyName);
            modelClientValidationRule.ValidationParameters.Add("type", TypeName.ToString());

            yield return modelClientValidationRule;
        }
    }
}