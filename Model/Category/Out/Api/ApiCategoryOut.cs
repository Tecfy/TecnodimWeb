using Model.VM;

namespace Model.Out
{
    public class ApiCategoryOut : ResultServiceVM
    {
        public ApiCategoryOut()
        {
            this.result = new ApiCategoryVM();
        }

        public ApiCategoryVM result { get; set; }
    }
}
