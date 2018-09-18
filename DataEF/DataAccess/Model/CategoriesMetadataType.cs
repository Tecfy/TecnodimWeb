using System;
using System.ComponentModel.DataAnnotations;

namespace DataEF.DataAccess.Model
{
    public partial class CategoriesMetadataType
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; } // CategoryId (Primary key)

        [Display(Name = "Active", ResourceType = typeof(i18n.Resource))]
        public bool Active { get; set; } // Active

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; } // CreatedDate

        [Display(Name = "EditedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? EditedDate { get; set; } // EditedDate

        [Display(Name = "DeletedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime? DeletedDate { get; set; } // DeletedDate

        [Display(Name = "Parent", ResourceType = typeof(i18n.Resource))]
        public int? ParentId { get; set; } // ParentId

        [Display(Name = "ExternalId", ResourceType = typeof(i18n.Resource))]
        public int ExternalId { get; set; } // ExternalId

        [StringLength(50, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public string Code { get; set; } // Code

        [StringLength(255, ErrorMessageResourceName = "MaxLengthMessage", ErrorMessageResourceType = typeof(i18n.Resource))]
        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; } // Name

        [Display(Name = "PB", ResourceType = typeof(i18n.Resource))]
        public bool Pb { get; set; } // PB

        [Display(Name = "Release", ResourceType = typeof(i18n.Resource))]
        public bool Release { get; set; } // Release
    }
}
