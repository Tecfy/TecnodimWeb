using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class AspNetUserClaimOut : ResultServiceVM
    {
        public AspNetUserClaimOut()
        {
            result = new List<AspNetUserClaimVM>();
        }

        public List<AspNetUserClaimVM> result { get; set; }
    }
}
