using System;
using System.Collections.Generic;

namespace Model.In
{
    public class ECMAttributeIn
    {
        public ECMAttributeIn(string externalId, List<ECMAttributeItemIn> itens)
        {
            this.externalId = externalId ?? throw new ArgumentNullException(nameof(externalId));
            this.itens = itens ?? throw new ArgumentNullException(nameof(itens));
        }

        public string externalId { get; set; }

        public List<ECMAttributeItemIn> itens { get; set; }
    }

    public class ECMAttributeItemIn
    {
        public string attribute { get; set; }

        public string value { get; set; }
    }
}
