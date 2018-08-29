using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ClassificationIn : BaseIn
    {
        [Required]
        [Display(Name = "Slice", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int sliceId { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int? categoryId { get; set; }

        [Display(Name = "Pages", ResourceType = typeof(i18n.Resource))]
        public List<SlicePagesIn> slicePages { get; set; }

        [Display(Name = "AdditionalFields", ResourceType = typeof(i18n.Resource))]
        public List<AdditionalFieldIn> additionalFields { get; set; }        
    }
}