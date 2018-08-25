using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class UserIn
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int UserId { get; set; }

        [StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "AspNetUser", ResourceType = typeof(i18n.Resource))]
        public string AspNetUserId { get; set; }

        [Required]
        [StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Role", ResourceType = typeof(i18n.Resource))]
        public string RoleId { get; set; }

        [Required]
        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "FirstName", ResourceType = typeof(i18n.Resource))]
        public string FirstName { get; set; }

        [Required]
        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "LastName", ResourceType = typeof(i18n.Resource))]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(i18n.Resource))]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "NewPassword", ResourceType = typeof(i18n.Resource))]
        public bool NewPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceName = "MessageLengthBetween", ErrorMessageResourceType = typeof(i18n.Resource), MinimumLength = 6)]
        [Display(Name = "Password", ResourceType = typeof(i18n.Resource))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(i18n.Resource))]
        [Compare("Password", ErrorMessageResourceName = "PasswordNotMatch", ErrorMessageResourceType = typeof(i18n.Resource))]
        public string ConfirmPassword { get; set; }
    }
}
