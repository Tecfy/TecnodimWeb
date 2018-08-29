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

        [Display(Name = "DocumentStatus", ResourceType = typeof(i18n.Resource))]
        public int DocumentStatusId { get; set; } // DocumentStatusId

        [Display(Name = "Unity", ResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; } // UnityId

        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "ExternalId", ResourceType = typeof(i18n.Resource))]
        public string ExternalId { get; set; } // ExternalId

        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Registration", ResourceType = typeof(i18n.Resource))]
        public string Registration { get; set; } // Registration

        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; } // Name

        [Display(Name = "Hash", ResourceType = typeof(i18n.Resource))]
        public Guid Hash { get; set; } // Hash
    }
}
