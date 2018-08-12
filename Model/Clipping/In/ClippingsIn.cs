using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ClippingsIn : BaseIn
    {
        [Required]
        [Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int documentId { get; set; }
    }
}
