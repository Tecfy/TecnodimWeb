using System.Collections.Generic;

namespace Model.VM
{

    public class AccessResultVM
    {
        public AccessResultVM()
        {
            userName = null;
            name = null;
            aspNetUserId = null;
            claims = null;
            access_token = null;
            token_type = null;
            expires = null;

            Units = new List<UnitsDDLVM>();
        }

        public string userName { get; set; }

        public string name { get; set; }

        public string aspNetUserId { get; set; }

        public string claims { get; set; }

        public string access_token { get; set; }

        public string token_type { get; set; }

        public string expires { get; set; }

        public List<UnitsDDLVM> Units { get; set; }
    }
}
