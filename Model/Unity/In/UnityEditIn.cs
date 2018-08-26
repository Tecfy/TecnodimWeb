using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class UnityEditIn
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "External", ResourceType = typeof(i18n.Resource))]
        public string ExternalId { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }
    }
}
