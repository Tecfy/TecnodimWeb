using System.Collections.Generic;

namespace Model.VM
{
    public class ResendDocumentsVM
    {
        public string registration { get; set; }

        public string cpf { get; set; }

        public string name { get; set; }

        public List<ResendDocumentItemVM> itens { get; set; }
    }
}
