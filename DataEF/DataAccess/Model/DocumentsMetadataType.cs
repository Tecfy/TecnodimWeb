using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class DocumentsMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int DocumentId { get; set; } // DocumentId (Primary key)

        [Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
        public bool Active { get; set; } // Active

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? EditedDate { get; set; } // EditedDate

        [Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        [Display(Name = "ExternalId", ResourceType = typeof(i18n.Resource))]
        public int ExternalId { get; set; } // ExternalId

        [Display(Name = "DocumentStatus", ResourceType = typeof(i18n.Resource))]
        public int DocumentStatusId { get; set; } // DocumentStatusId

        [Display(Name = "Hash", ResourceType = typeof(i18n.Resource))]
        public Guid Hash { get; set; } // Hash
    }
}
