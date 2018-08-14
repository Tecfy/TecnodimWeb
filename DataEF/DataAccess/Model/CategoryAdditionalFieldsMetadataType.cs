using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class CategoryAdditionalFieldsMetadataType
    {
		[Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
		public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId (Primary key)

		[Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
		public int CategoryId { get; set; } // CategoryId

		[Display(Name = "AdditionalField", ResourceType = typeof(i18n.Resource))]
		public int AdditionalFieldId { get; set; } // AdditionalFieldId

		[Display(Name = "Single", ResourceType = typeof(i18n.Resource))]
		public bool Single { get; set; } // Single

		[Display(Name = "Required", ResourceType = typeof(i18n.Resource))]
		public bool Required { get; set; } // Required

		[Display(Name = "Confidential", ResourceType = typeof(i18n.Resource))]
		public bool Confidential { get; set; } // Confidential
    }
}
