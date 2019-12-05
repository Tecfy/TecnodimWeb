using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ResendDocumentItemIn : BaseIn
    {
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "URL", ResourceType = typeof(i18n.Resource))]
        public string uri { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Title", ResourceType = typeof(i18n.Resource))]
        public string title { get; set; }
    }
}
