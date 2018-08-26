using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class CategoryCreateIn
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Parent", ResourceType = typeof(i18n.Resource))]
        public int? ParentId { get; set; }

        [Required]
        [Display(Name = "External", ResourceType = typeof(i18n.Resource))]
        public int ExternalId { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public string Code { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }
    }
}
