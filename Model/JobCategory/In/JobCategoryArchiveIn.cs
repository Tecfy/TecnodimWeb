﻿using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobCategoryArchiveIn : BaseIn
    {
        [Required]
        [Display(Name = "JobCategory", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobCategoryId { get; set; }

        [Required]
        [Display(Name = "Archive", ResourceType = typeof(i18n.Resource))]
        public string archive { get; set; }
    }
}