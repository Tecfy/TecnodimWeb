using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentDetailIn : BaseIn
    {
        [Required]
        [Display(Name = "DocumentId", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int documentId { get; set; }

        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string registration { get; set; }
    }
}
