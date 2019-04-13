namespace Model
{
    public class BaseIn : PaginateIn
    {
        public BaseIn()
        {
            id = null;
            key = null;
        }

        public string id { get; set; }

        public string key { get; set; }
    }
}
