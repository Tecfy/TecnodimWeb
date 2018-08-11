using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class PageIn
    {
        [Required]
        [Display(Name = "Index", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int index { get; set; }

        [Required]
        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int pageId { get; set; }

        [Display(Name = "Rotation", ResourceType = typeof(i18n.Resource))]
        public int? rotation { get; set; }
    }
}
