using System;

namespace Model.VM
{
    public class SyncRuntimesVM
    {
        public int SyncRuntimeId { get; set; }

        public string URL { get; set; }

        public int Interval { get; set; }

        public DateTime LastExecution { get; set; }
    }
}
