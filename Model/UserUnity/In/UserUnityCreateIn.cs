using System.ComponentModel.DataAnnotations;

namespace Model.In
{
    public class UserUnityCreateIn
    {
        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; }

        [Display(Name = "User", ResourceType = typeof(i18n.Resource))]
        public int UserId { get; set; }
    }
}
