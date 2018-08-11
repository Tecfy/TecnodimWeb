using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class ClippingsMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int ClippingId { get; set; } // ClippingId (Primary key)

		[Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
		public bool Active { get; set; } // Active

		[Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime CreatedDate { get; set; } // CreatedDate

		[Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? EditedDate { get; set; } // EditedDate

		[Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
		public DateTime? DeletedDate { get; set; } // DeletedDate

		[Display(Name = "Document", ResourceType = typeof(i18n.Resource))]
		public int DocumentId { get; set; } // DocumentId

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
		public string Name { get; set; } // Name

		[StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
		[Display(Name = "Pages", ResourceType = typeof(i18n.Resource))]
		public string Pages { get; set; } // Pages
    }
}
