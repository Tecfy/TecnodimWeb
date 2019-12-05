using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ResendDocumentIn : BaseIn
    {
        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
        public string documentId { get; set; }

        [Required]
        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int unityId { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string registration { get; set; }

        public List<ResendDocumentItemIn> itens { get; set; }
    }
}
