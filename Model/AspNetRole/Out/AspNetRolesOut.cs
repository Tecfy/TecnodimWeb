using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class AspNetRolesOut : ResultServiceVM
    {
        public AspNetRolesOut()
        {
            result = new List<AspNetRolesVM>();
        }

        public List<AspNetRolesVM> result { get; set; }
    }
}
