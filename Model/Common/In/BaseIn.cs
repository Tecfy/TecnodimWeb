namespace Model
{
    public class BaseIn : PaginateIn
    {
        public BaseIn()
        {
            this.id = null;
            this.key = null;
        }

        public string id { get; set; }

        public string key { get; set; }
    }
}
