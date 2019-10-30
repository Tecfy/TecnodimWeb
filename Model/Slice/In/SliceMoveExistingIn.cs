using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class SliceMoveExistingIn : BaseIn
    {
        [Required]
        [Display(Name = "Slice", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int sliceNewId { get; set; }

        [Required]
        [Display(Name = "Slice", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int sliceOldId { get; set; }

        [Required]
        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredFieldInt", ErrorMessageResourceType = typeof(i18n.Resource))]
        public int page { get; set; }
    }
}
