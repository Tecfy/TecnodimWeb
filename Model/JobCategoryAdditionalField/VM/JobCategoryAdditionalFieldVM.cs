namespace Model.VM
{
    public class JobCategoryAdditionalFieldVM
    {
        public int jobCategoryAdditionalFieldId { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public string value { get; set; }

        public bool single { get; set; }

        public bool required { get; set; }

        public bool confidential { get; set; }
    }
}
