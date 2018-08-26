using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class CategoriesDDLVM
    {
        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(i18n.Resource))]
        public string Name { get; set; }
    }
}
