namespace Model.VM
{
    public class CategoryAdditionalFieldEditVM : ResultServiceVM
    {
        public int CategoryAdditionalFieldId { get; set; }

        public int CategoryId { get; set; }

        public int AdditionalFieldId { get; set; }

        public bool Single { get; set; }

        public bool Required { get; set; }
    }
}
