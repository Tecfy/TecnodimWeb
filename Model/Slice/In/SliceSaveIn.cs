using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class SliceSaveIn : BaseIn
    {
        [Required]
        [Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int externalId { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string name { get; set; }

        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        public List<PageSliceIn> pages { get; set; }
    }
}
