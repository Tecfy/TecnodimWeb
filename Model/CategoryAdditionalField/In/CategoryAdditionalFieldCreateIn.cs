using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class CategoryAdditionalFieldCreateIn : BaseIn
    {        
        [Required]
        [Display(Name = "CategoryAdditionalField", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int AdditionalFieldId { get; set; }

        [Display(Name = "Single", ResourceType = typeof(i18n.Resource))]
        public bool Single { get; set; }

        [Display(Name = "Required", ResourceType = typeof(i18n.Resource))]
        public bool Required { get; set; }
    }
}
