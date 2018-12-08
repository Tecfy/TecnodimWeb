using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobCategoryIncludeIn : BaseIn
    {
        [Required]
        [Display(Name = "Job", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobId { get; set; }

        [Required]
        [Display(Name = "JobCategory", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int categoryId { get; set; }
    }
}