using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class PageClippingIn
    {
        [Required]
        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int page { get; set; }
    }
}
