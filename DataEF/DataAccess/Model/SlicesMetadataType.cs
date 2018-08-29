using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class SlicesMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int SliceId { get; set; } // SliceId (Primary key)

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

        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        public int? CategoryId { get; set; } // CategoryId

        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; } // Name

        [Display(Name = "Sent", ResourceType = typeof(i18n.Resource))]
        public bool Sent { get; set; } // Sent

        [Display(Name = "Sending", ResourceType = typeof(i18n.Resource))]
        public bool Sending { get; set; } // Sending

        [Display(Name = "SendingDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? SendingDate { get; set; } // SendingDate
    }
}
