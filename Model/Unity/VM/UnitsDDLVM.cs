using System;
using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class UnitsDDLVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int UnityId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }
    }
}
