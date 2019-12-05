using System.Collections.Generic;

namespace Model.VM
{
    public class ResendDocumentsVM
    {
        public string documentId { get; set; }

        public string registration { get; set; }

        public string cpf { get; set; }

        public string name { get; set; }

        public string title { get; set; }

        public List<ResendDocumentsItemVM> itens { get; set; }
    }
}
