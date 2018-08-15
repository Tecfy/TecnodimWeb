using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class DeletedPagesMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int DeletedPageId { get; set; } // DeletedPageId (Primary key)

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

        [Display(Name = "Page", ResourceType = typeof(i18n.Resource))]
        public int Page { get; set; } // Page
    }
}
