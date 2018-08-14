using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ClassificationIn : BaseIn
    {
        [Required]
        [Display(Name = "Clipping", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int clippingId { get; set; }

        [Required]
        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int? categoryId { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string name { get; set; }

        [Required]        
        [Display(Name = "Classification", ResourceType = typeof(i18n.Resource))]
        public bool classification { get; set; }


        [Display(Name = "Pages", ResourceType = typeof(i18n.Resource))]
        public List<ClippingPagesIn> pages { get; set; }
    }
}