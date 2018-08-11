using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class ClippingsVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int clippingId { get; set; }

        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string name { get; set; }

        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Pages", ResourceType = typeof(i18n.Resource))]
        public string pages { get; set; }
    }
}
