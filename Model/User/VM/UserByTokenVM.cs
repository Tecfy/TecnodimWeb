using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class UserByTokenVM
    {
        [Display(Name = "ExternalUser", ResourceType = typeof(i18n.Resource))]
        public string Registration { get; set; }
    }
}
