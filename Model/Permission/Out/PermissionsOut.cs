using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class PermissionsOut : ResultServiceVM
    {
        public PermissionsOut()
        {
            this.result = new List<PermissionsVM>();
        }

        public List<PermissionsVM> result { get; set; }
    }
}
