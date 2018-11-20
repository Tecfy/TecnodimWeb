using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class WorkCategoryIn : BaseIn
    {
        [Required]
        [Display(Name = "WorkCategory", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int workCategoryId { get; set; }

        [Required]
        [Display(Name = "Archive", ResourceType = typeof(i18n.Resource))]
        public string archive { get; set; }
    }
}