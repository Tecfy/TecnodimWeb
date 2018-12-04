using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobCategoryIn : BaseIn
    {
        [Required]
        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int categoryId { get; set; }
    }
}