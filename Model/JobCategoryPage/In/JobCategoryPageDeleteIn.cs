using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobCategoryPageDeleteIn : BaseIn
    {
        [Required]
        [Display(Name = "JobCategoryPage", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobCategoryPageId { get; set; }
    }
}
