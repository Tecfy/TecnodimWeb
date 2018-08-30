using System.ComponentModel.DataAnnotations;

namespace Model.VM
{
    public class CategoryAdditionalFieldVM : ResultServiceVM
    {
        public CategoryAdditionalFieldVM()
        {
        }

        public CategoryAdditionalFieldVM(int categoryId, int additionalFieldId)
        {
            CategoryId = categoryId;
            AdditionalFieldId = additionalFieldId;
        }

        [Display(Name = "Code", ResourceType = typeof(i18n.Resource))]
        public int? CategoryAdditionalFieldId { get; set; }

        [Display(Name = "Category", ResourceType = typeof(i18n.Resource))]
        public int CategoryId { get; set; }

        [Display(Name = "AdditionalField", ResourceType = typeof(i18n.Resource))]
        public int AdditionalFieldId { get; set; }

        [Display(Name = "Single", ResourceType = typeof(i18n.Resource))]
        public bool Single { get; set; }

        [Display(Name = "Required", ResourceType = typeof(i18n.Resource))]
        public bool Required { get; set; }
    }
}
