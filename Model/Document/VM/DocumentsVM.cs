using System;

namespace Model.VM
{
    public class DocumentsVM
    {
        public int documentId { get; set; }

        public string name { get; set; }

        public string registration { get; set; }

        public int statusId { get; set; }

        public string status { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }
}
