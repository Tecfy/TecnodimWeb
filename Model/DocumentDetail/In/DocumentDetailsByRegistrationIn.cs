using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class DocumentDetailsByRegistrationIn : BaseIn
    {
        [Required]
        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string Registration { get; set; }

        [Required]
        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; }
    }
}
