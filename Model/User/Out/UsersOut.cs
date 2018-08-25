using Model.VM;
using System.Collections.Generic;

namespace Model.Out
{
    public class UsersOut : ResultServiceVM
    {
        public UsersOut()
        {
            this.result = new List<UsersVM>();
        }

        public List<UsersVM> result { get; set; }

        public int? totalCount { get; set; }
    }
}
