using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class RegisterEventsMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int RegisterEventId { get; set; } // RegisterEventId (Primary key)

        [Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
        public bool Active { get; set; } // Active

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? EditedDate { get; set; } // EditedDate

        [Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        [StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "UserId", ResourceType = typeof(i18n.Resource))]
        public string UserId { get; set; } // UserId

        [StringLength(128, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Identifier", ResourceType = typeof(i18n.Resource))]
        public string Identifier { get; set; } // Identifier

        [StringLength(100, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Type", ResourceType = typeof(i18n.Resource))]
        public string Type { get; set; } // Type

        [Display(Name = "Source", ResourceType = typeof(i18n.Resource))]
        public string Source { get; set; } // Source

        [Display(Name = "Text", ResourceType = typeof(i18n.Resource))]
        public string Text { get; set; } // Text
    }
}
