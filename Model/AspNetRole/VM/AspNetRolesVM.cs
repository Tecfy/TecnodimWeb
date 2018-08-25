using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class AspNetRolesVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public string RoleId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }
    }
}
