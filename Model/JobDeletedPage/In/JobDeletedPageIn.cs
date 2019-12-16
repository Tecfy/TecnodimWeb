using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobDeletedPageIn : BaseIn
    {
        [Required]
        [Display(Name = "JobCategory", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobCategoryId { get; set; }

        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        public List<JobPageDeletedPageIn> pages { get; set; }
    }
}
