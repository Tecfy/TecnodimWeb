using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class SlicePageDeleteIn : BaseIn
    {
        [Required]
        [Display(Name = "SlicePage", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int slicePageId { get; set; }
    }
}
