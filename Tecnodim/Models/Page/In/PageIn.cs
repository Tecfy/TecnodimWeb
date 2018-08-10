using System.ComponentModel.DataAnnotations;

namespace Tecnodim.Models.In
{
    public class PageIn
    {
        [Required]
        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int PageId { get; set; }

        [Display(Name = "Rotation", ResourceType = typeof(i18n.Resource))]
        public int? Rotation { get; set; }
    }
}