namespace Model.VM
{
    public class CategoryEditVM
    {
        public int CategoryId { get; set; }

        public int? ParentId { get; set; }

        public int ExternalId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
    }
}
