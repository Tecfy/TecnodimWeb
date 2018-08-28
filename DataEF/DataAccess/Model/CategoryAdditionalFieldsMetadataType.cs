using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class CategoryAdditionalFieldsMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int CategoryAdditionalFieldId { get; set; } // CategoryAdditionalFieldId (Primary key)

        [Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
        public bool Active { get; set; } // Active

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? EditedDate { get; set; } // EditedDate

        [Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; } // CategoryId

        [Display(Name = "AdditionalField", ResourceType = typeof(i18n.Resource))]
        public int AdditionalFieldId { get; set; } // AdditionalFieldId

        [Display(Name = "Single", ResourceType = typeof(i18n.Resource))]
        public bool Single { get; set; } // Single

        [Display(Name = "Required", ResourceType = typeof(i18n.Resource))]
        public bool Required { get; set; } // Required
    }
}
