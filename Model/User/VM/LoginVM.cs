using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class LoginVM
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(i18n.Resource))]        
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(i18n.Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember", ResourceType = typeof(i18n.Resource))]
        public bool RememberMe { get; set; }
    }
}
