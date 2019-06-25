using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class PermissionOut : ResultServiceVM
    {
        public PermissionOut()
        {
            result = new List<PermissionVM>();
        }

        public List<PermissionVM> result { get; set; }
    }
}
