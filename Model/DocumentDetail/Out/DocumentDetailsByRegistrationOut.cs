using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class DocumentDetailsByRegistrationOut : ResultServiceVM
    {
        public DocumentDetailsByRegistrationOut()
        {
            this.result = new List<DocumentDetailsByRegistrationVM>();
        }

        public List<DocumentDetailsByRegistrationVM> result { get; set; }
    }
}
