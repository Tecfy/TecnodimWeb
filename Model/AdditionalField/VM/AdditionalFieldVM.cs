namespace Model.VM
{
    public class AdditionalFieldVM
    {
        public int categoryAdditionalFieldId { get; set; }

        public string name { get; set; }

        public string type { get; set; }

        public bool single { get; set; }

        public bool required { get; set; }

        public bool confidential { get; set; }
    }
}
