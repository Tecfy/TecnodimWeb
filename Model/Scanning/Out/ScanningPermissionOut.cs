using System;

namespace Model.Out
{
    public class ScanningPermissionOut : ResultServiceVM
    {
        public ScanningPermissionOut()
        {
        }

        public static implicit operator ScanningPermissionOut(ScanningOut v)
        {
            throw new NotImplementedException();
        }
    }
}
