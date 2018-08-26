using System;
using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class UnitsVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; }

        [Display(Name = "External", ResourceType = typeof(i18n.Resource))]
        public string ExternalId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; }
    }
}
