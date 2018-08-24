using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class UserUnitsMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int UserUnityId { get; set; } // UserUnityId (Primary key)

		[Display(Name = "User", ResourceType = typeof(i18n.Resource))]
		public int UserId { get; set; } // UserId

		[Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
		public int UnityId { get; set; } // UnityId
    }
}
