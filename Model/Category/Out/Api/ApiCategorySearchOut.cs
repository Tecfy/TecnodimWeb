using Model.VM;

namespace Model.Out
{
    public class ApiCategorySearchOut : ResultServiceVM
    {
        public ApiCategorySearchOut()
        {
            this.result = new ApiCategorySearchVM();
        }

        public ApiCategorySearchVM result { get; set; }
    }
}
