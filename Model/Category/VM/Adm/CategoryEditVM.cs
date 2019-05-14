namespace Model.VM
{
    public class CategoryEditVM
    {
        public int CategoryId { get; set; }

        public string Parent { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public bool pb { get; set; }

        public bool pbEmbarked { get; set; }

        public bool Release { get; set; }


        public bool ShowIdentifier { get; set; }

        public bool ShowCompetence { get; set; }

        public bool ShowValidity { get; set; }

        public bool ShowDocumentView { get; set; }

        public bool ShowNote { get; set; }


        public CategoryAdditionalFieldVM Identifier { get; set; }

        public CategoryAdditionalFieldVM Competence { get; set; }

        public CategoryAdditionalFieldVM Validity { get; set; }

        public CategoryAdditionalFieldVM DocumentView { get; set; }

        public CategoryAdditionalFieldVM Note { get; set; }
    }
}
