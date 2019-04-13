using Model.VM;

namespace Model.Out
{
    public class UserByTokenOut : ResultServiceVM
    {
        public UserByTokenOut()
        {
            result = new UserByTokenVM();
        }

        public UserByTokenVM result { get; set; }
    }
}
