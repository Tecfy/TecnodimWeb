using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class ApiUsersMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int ApiUserId { get; set; } // ApiUserId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[StringLength(250, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "User", ResourceType = typeof(i18n.Resource))]
		public string User { get; set; } // User

		[Display(Name = "Hash", ResourceType = typeof(i18n.Resource))]
		public Guid Hash { get; set; } // Hash

		[Display(Name = "LastLogin", ResourceType = typeof(i18n.Resource))]
		public DateTime? LastLogin { get; set; } // LastLogin
    }
}
