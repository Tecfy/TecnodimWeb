using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentDetailIn : BaseIn
    {
        [Required]
        [Display(Name = "External", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int externalId { get; set; }
    }
}
