﻿using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class SliceDeleteIn : BaseIn
    {
        [Required]
        [Display(Name = "Slice", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int sliceId { get; set; }
    }
}
