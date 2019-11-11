using System.Collections.Generic;

namespace Model.VM
{
    public class ResendDocumentsVM
    {
        public string id { get; set; }

        public string registration { get; set; }

        public string cpf { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public List<ResendDocumentItemVM> itens { get; set; }
    }
}
