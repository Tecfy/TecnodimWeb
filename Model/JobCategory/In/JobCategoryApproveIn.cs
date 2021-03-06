﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class JobCategoryApproveIn : BaseIn
    {
        [Required]
        [Display(Name = "JobCategory", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int jobCategoryId { get; set; }

        [Display(Name = "AdditionalFields", ResourceType = typeof(i18n.Resource))]
        public List<JobCategoryAdditionalFieldUpdateIn> additionalFields { get; set; }
    }
}