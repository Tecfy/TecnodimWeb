using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class ClippingPagesMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int ClippingPageId { get; set; } // ClippingPageId (Primary key)

        [Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
        public bool Active { get; set; } // Active

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? EditedDate { get; set; } // EditedDate

        [Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        [Display(Name = "ClippingId", ResourceType = typeof(i18n.Resource))]
        public int ClippingId { get; set; } // ClippingId

        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        public int Page { get; set; } // Page

        [Display(Name = "Rotate", ResourceType = typeof(i18n.Resource))]
        public int? Rotate { get; set; } // Rotate
    }
}
