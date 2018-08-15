using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class AdditionalFieldIn
    {
        [Required]
        [Display(Name = "CategoryAdditionalField", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int categoryAdditionalFieldId { get; set; }

        [Display(Name = "Value", ResourceType = typeof(i18n.Resource))]
        public string value { get; set; }
    }
}
