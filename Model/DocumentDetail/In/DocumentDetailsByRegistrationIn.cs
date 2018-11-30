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
        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        public string Unity { get; set; }
    }
}
