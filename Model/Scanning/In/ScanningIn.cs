using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class ScanningIn : BaseIn
    {
        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string registration { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string name { get; set; }


        [Display(Name = "JobCategories", ResourceType = typeof(i18n.Resource))]
        public List<JobCategoryIn> jobCategories { get; set; }
    }
}