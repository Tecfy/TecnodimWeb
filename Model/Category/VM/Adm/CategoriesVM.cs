using System;
using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class CategoriesVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }

        [Display(Name = "Parent", ResourceType = typeof(i18n.Resource))]
        public string Parent { get; set; }

        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public string Code { get; set; }

        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(i18n.Resource))]
        public DateTime CreatedDate { get; set; }
    }
}
