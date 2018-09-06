using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentUpdateIn : BaseIn
    {
        [Required]
        [Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int documentId { get; set; }

        [Required]
        [Display(Name = "Status", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int documentStatusId { get; set; }
    }
}
