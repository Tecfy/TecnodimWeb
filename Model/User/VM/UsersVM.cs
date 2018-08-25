using System;
using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class UsersVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int UserId { get; set; }

        [Display(Name = "Role", ResourceType = typeof(i18n.Resource))]
        public string Role { get; set; }

        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType = typeof(i18n.Resource))]
        public string Email { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; }
    }
}
