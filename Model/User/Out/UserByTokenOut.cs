using Model.VM;

namespace Model.Out
{
    public class UserByTokenOut : ResultServiceVM
    {
        public UserByTokenOut()
        {
            this.result = new UserByTokenVM();
        }

        public UserByTokenVM result { get; set; }
    }
}
